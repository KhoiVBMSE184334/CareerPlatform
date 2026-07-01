using System.Text;
using CareerPlatform.Application.DTOs.Chats;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Services;

public class AIChatService : IAIChatService
{
    private const string UserRole = "User";
    private const string AssistantRole = "Assistant";

    private readonly IUnitOfWork _unitOfWork;
    private readonly IGeminiService _geminiService;

    public AIChatService(IUnitOfWork unitOfWork, IGeminiService geminiService)
    {
        _unitOfWork = unitOfWork;
        _geminiService = geminiService;
    }

    public async Task<ChatResponseDto> SendMessageAsync(
        Guid userId,
        ChatRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User was not found.");
        }

        ChatSession? session = null;

        if (request.SessionId.HasValue)
        {
            session = await _unitOfWork.Chats.GetSessionByIdAsync(request.SessionId.Value, userId, cancellationToken);

            if (session is null)
            {
                throw new KeyNotFoundException("Chat session was not found.");
            }
        }

        var selectedCareerPath = await _unitOfWork.UserCareerPaths.GetByUserIdAsync(userId, cancellationToken);
        var userSkills = await _unitOfWork.UserSkills.GetByUserIdAsync(userId, cancellationToken);
        var roadmapProgress = await _unitOfWork.RoadmapProgresses.GetByUserIdAsync(userId, cancellationToken);
        var portfolioProjects = await _unitOfWork.PortfolioProjects.GetByUserIdAsync(userId, cancellationToken);
        var skillNodes = selectedCareerPath is null
            ? []
            : await _unitOfWork.SkillNodes.GetByCareerPathIdAsync(selectedCareerPath.CareerPathId, cancellationToken);
        var previousMessages = session?.ChatMessages
            .OrderBy(message => message.CreatedAt)
            .ToList() ?? [];

        var mentorPrompt = BuildMentorPrompt(
            user,
            selectedCareerPath?.CareerPath,
            userSkills,
            skillNodes,
            roadmapProgress,
            portfolioProjects,
            previousMessages,
            request.Message);

        var mentorResponse = await _geminiService.GenerateMentorResponseAsync(
            mentorPrompt,
            cancellationToken);

        if (session is null)
        {
            session = new ChatSession
            {
                SessionId = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Chats.AddSessionAsync(session, cancellationToken);
        }

        var now = DateTime.UtcNow;

        await _unitOfWork.Chats.AddMessageAsync(new ChatMessage
        {
            MessageId = Guid.NewGuid(),
            SessionId = session.SessionId,
            Role = UserRole,
            Content = request.Message.Trim(),
            CreatedAt = now
        }, cancellationToken);

        var assistantMessage = new ChatMessage
        {
            MessageId = Guid.NewGuid(),
            SessionId = session.SessionId,
            Role = AssistantRole,
            Content = mentorResponse,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Chats.AddMessageAsync(assistantMessage, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ChatResponseDto
        {
            SessionId = session.SessionId,
            Response = assistantMessage.Content,
            CreatedAt = assistantMessage.CreatedAt
        };
    }

    public async Task<IReadOnlyList<ChatSessionDto>> GetSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var sessions = await _unitOfWork.Chats.GetSessionsByUserIdAsync(userId, cancellationToken);
        return sessions.Select(MapSession).ToList();
    }

    public async Task<ChatSessionDto?> GetSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _unitOfWork.Chats.GetSessionByIdAsync(sessionId, userId, cancellationToken);
        return session is null ? null : MapSession(session);
    }

    private static ChatSessionDto MapSession(ChatSession session)
    {
        return new ChatSessionDto
        {
            SessionId = session.SessionId,
            CreatedAt = session.CreatedAt,
            Messages = session.ChatMessages
                .OrderBy(message => message.CreatedAt)
                .Select(message => new ChatMessageDto
                {
                    MessageId = message.MessageId,
                    Role = message.Role,
                    Content = message.Content,
                    CreatedAt = message.CreatedAt
                })
                .ToList()
        };
    }

    private static string BuildMentorPrompt(
        User user,
        CareerPath? selectedCareerPath,
        IReadOnlyList<UserSkill> userSkills,
        IReadOnlyList<SkillNode> skillNodes,
        IReadOnlyList<RoadmapProgress> roadmapProgress,
        IReadOnlyList<PortfolioProject> portfolioProjects,
        IReadOnlyList<ChatMessage> previousMessages,
        string userMessage)
    {
        var currentSkillNames = userSkills
            .Select(userSkill => userSkill.SkillName.Trim())
            .Where(skillName => !string.IsNullOrWhiteSpace(skillName))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(skillName => skillName)
            .ToList();

        var completedSkillNodeIds = roadmapProgress
            .Where(progress => progress.IsCompleted)
            .Select(progress => progress.SkillNodeId)
            .ToHashSet();

        var completedSkillNames = skillNodes
            .OrderBy(skillNode => skillNode.DisplayOrder)
            .Where(skillNode => completedSkillNodeIds.Contains(skillNode.SkillNodeId))
            .Select(skillNode => skillNode.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var missingSkillNames = skillNodes
            .OrderBy(skillNode => skillNode.DisplayOrder)
            .Where(skillNode => !completedSkillNodeIds.Contains(skillNode.SkillNodeId))
            .Select(skillNode => skillNode.Name)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var roadmapCompletionPercentage = CalculateRoadmapCompletion(skillNodes, roadmapProgress);

        var projectNames = portfolioProjects
            .OrderByDescending(project => project.ImportedAt)
            .Select(project => project.RepositoryName)
            .Where(repositoryName => !string.IsNullOrWhiteSpace(repositoryName))
            .ToList();

        var previousConversation = previousMessages
            .OrderBy(chatMessage => chatMessage.CreatedAt)
            .TakeLast(12)
            .Select(chatMessage => $"{chatMessage.Role}: {chatMessage.Content}");

        var profile = user.StudentProfile;
        var prompt = new StringBuilder();

        prompt.AppendLine("Student Profile:");
        prompt.AppendLine($"Name: {user.FullName}");
        prompt.AppendLine($"Email: {user.Email}");
        prompt.AppendLine($"University: {profile?.University ?? "Not provided"}");
        prompt.AppendLine($"Major: {profile?.Major ?? "Not provided"}");
        prompt.AppendLine($"GPA: {(profile?.GPA.HasValue == true ? profile.GPA.Value.ToString("0.00") : "Not provided")}");
        prompt.AppendLine($"GitHub: {profile?.GithubUrl ?? "Not provided"}");
        prompt.AppendLine($"Career Path: {selectedCareerPath?.Name ?? "Not selected"}");
        prompt.AppendLine($"Career Path Description: {selectedCareerPath?.Description ?? "Not provided"}");
        prompt.AppendLine();

        prompt.AppendLine("Current Skills:");
        prompt.AppendLine(FormatList(currentSkillNames));
        prompt.AppendLine();

        prompt.AppendLine("Completed Skills:");
        prompt.AppendLine(FormatList(completedSkillNames));
        prompt.AppendLine();

        prompt.AppendLine("Missing Skills (ordered by roadmap priority):");
        prompt.AppendLine(FormatList(missingSkillNames));
        prompt.AppendLine();

        prompt.AppendLine("Recommended order:");
        prompt.AppendLine(FormatNumberedList(missingSkillNames));
        prompt.AppendLine();

        prompt.AppendLine("Roadmap Progress:");
        prompt.AppendLine($"{roadmapCompletionPercentage}%");
        prompt.AppendLine();

        prompt.AppendLine("Portfolio Projects:");
        prompt.AppendLine(FormatList(projectNames));
        prompt.AppendLine();

        prompt.AppendLine("Recent Chat History:");
        prompt.AppendLine(previousConversation.Any()
            ? string.Join(Environment.NewLine, previousConversation)
            : "No previous messages in this session.");
        prompt.AppendLine();

        prompt.AppendLine("Instructions:");
        prompt.AppendLine("You are a Software Engineering career mentor.");
        prompt.AppendLine("Use the student's actual career path, roadmap progress, current skills, missing skills and portfolio projects.");
        prompt.AppendLine("Do not give generic advice.");
        prompt.AppendLine("Never recommend a skill that already exists in Completed Skills.");
        prompt.AppendLine("Always prioritize recommendations from Missing Skills.");
        prompt.AppendLine("Sort recommendations by roadmap order using the Recommended order section.");
        prompt.AppendLine("If Missing Skills is empty, recommend advanced topics related to the career path.");
        prompt.AppendLine("Recommend projects relevant to the selected career path.");
        prompt.AppendLine("Keep answers practical and career-focused.");
        prompt.AppendLine("Keep the response concise and beginner-friendly.");
        prompt.AppendLine();

        prompt.AppendLine("Student Message:");
        prompt.AppendLine(userMessage.Trim());

        return prompt.ToString();
    }

    private static decimal CalculateRoadmapCompletion(
        IReadOnlyList<SkillNode> skillNodes,
        IReadOnlyList<RoadmapProgress> roadmapProgress)
    {
        if (skillNodes.Count == 0)
        {
            return 0;
        }

        var skillNodeIds = skillNodes.Select(skillNode => skillNode.SkillNodeId).ToHashSet();
        var completedCount = roadmapProgress.Count(progress =>
            progress.IsCompleted && skillNodeIds.Contains(progress.SkillNodeId));

        return Math.Round((decimal)completedCount / skillNodes.Count * 100, 2);
    }

    private static string FormatList(IReadOnlyList<string> values)
    {
        return values.Count == 0
            ? "None provided"
            : string.Join(Environment.NewLine, values.Select(value => $"- {value}"));
    }

    private static string FormatNumberedList(IReadOnlyList<string> values)
    {
        return values.Count == 0
            ? "No missing roadmap skills. Recommend advanced topics related to the selected career path."
            : string.Join(Environment.NewLine, values.Select((value, index) => $"{index + 1}. {value}"));
    }
}
