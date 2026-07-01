using CareerPlatform.Application.DTOs.CareerPaths;
using CareerPlatform.Application.DTOs.Chats;
using CareerPlatform.Application.DTOs.Dashboards;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;
using CareerPlatform.Domain.Enums;

namespace CareerPlatform.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;

    public DashboardService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<StudentDashboardDto> GetStudentDashboardAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var selectedCareerPath = await _unitOfWork.UserCareerPaths.GetByUserIdAsync(userId, cancellationToken);
        var recentChatSessions = await _unitOfWork.Chats.GetSessionsByUserIdAsync(userId, cancellationToken);
        var portfolioProjects = await _unitOfWork.PortfolioProjects.GetByUserIdAsync(userId, cancellationToken);

        if (selectedCareerPath is null)
        {
            return new StudentDashboardDto
            {
                RecentAIChatSessions = recentChatSessions.Take(5).Select(MapChatSession).ToList(),
                PortfolioProjectCount = portfolioProjects.Count
            };
        }

        var careerPath = selectedCareerPath.CareerPath;
        var skillNodes = await _unitOfWork.SkillNodes.GetByCareerPathIdAsync(selectedCareerPath.CareerPathId, cancellationToken);
        var progressRecords = await _unitOfWork.RoadmapProgresses.GetByUserIdAsync(userId, cancellationToken);
        var userSkills = await _unitOfWork.UserSkills.GetByUserIdAsync(userId, cancellationToken);

        var roadmapCompletionPercentage = CalculateRoadmapCompletion(skillNodes, progressRecords);
        var matchedSkillNames = userSkills
            .Select(userSkill => NormalizeSkill(userSkill.SkillName))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var missingSkillNodes = skillNodes
            .OrderBy(skillNode => skillNode.DisplayOrder)
            .Where(skillNode => !matchedSkillNames.Contains(NormalizeSkill(skillNode.Name)))
            .ToList();

        var skillMatchPercentage = skillNodes.Count == 0
            ? 0
            : Math.Round((decimal)(skillNodes.Count - missingSkillNodes.Count) / skillNodes.Count * 100, 2);

        return new StudentDashboardDto
        {
            SelectedCareerPath = new CareerPathDto
            {
                CareerPathId = careerPath.CareerPathId,
                Name = careerPath.Name,
                Description = careerPath.Description
            },
            RoadmapCompletionPercentage = roadmapCompletionPercentage,
            SkillMatchPercentage = skillMatchPercentage,
            MissingSkillsSummary = missingSkillNodes.Select(skillNode => skillNode.Name).ToList(),
            RecentAIChatSessions = recentChatSessions.Take(5).Select(MapChatSession).ToList(),
            PortfolioProjectCount = portfolioProjects.Count
        };
    }

    public async Task<AdminDashboardDto> GetAdminDashboardAsync(CancellationToken cancellationToken = default)
    {
        return new AdminDashboardDto
        {
            TotalUsers = await _unitOfWork.Users.CountAsync(cancellationToken),
            TotalStudents = await _unitOfWork.Users.CountByRoleAsync(UserRole.Student, cancellationToken),
            TotalAdmins = await _unitOfWork.Users.CountByRoleAsync(UserRole.Admin, cancellationToken),
            TotalCareerPaths = await _unitOfWork.CareerPaths.CountAsync(cancellationToken),
            TotalSkillNodes = await _unitOfWork.SkillNodes.CountAsync(cancellationToken),
            TotalPortfolioProjects = await _unitOfWork.PortfolioProjects.CountAsync(cancellationToken)
        };
    }

    private static decimal CalculateRoadmapCompletion(
        IReadOnlyList<SkillNode> skillNodes,
        IReadOnlyList<RoadmapProgress> progressRecords)
    {
        if (skillNodes.Count == 0)
        {
            return 0;
        }

        var skillNodeIds = skillNodes.Select(skillNode => skillNode.SkillNodeId).ToHashSet();
        var completedCount = progressRecords.Count(progress =>
            progress.IsCompleted && skillNodeIds.Contains(progress.SkillNodeId));

        return Math.Round((decimal)completedCount / skillNodes.Count * 100, 2);
    }

    private static ChatSessionDto MapChatSession(ChatSession session)
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

    private static string NormalizeSkill(string skill)
    {
        return skill.Trim().ToLowerInvariant();
    }
}
