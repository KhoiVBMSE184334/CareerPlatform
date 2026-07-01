using CareerPlatform.Application.DTOs.SkillGaps;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Services;

public class SkillGapService : ISkillGapService
{
    private readonly IUnitOfWork _unitOfWork;

    public SkillGapService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SkillGapResultDto> AnalyzeAsync(
        Guid userId,
        SkillGapRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var careerPathId = await ResolveCareerPathIdAsync(userId, request.CareerPathId, cancellationToken);
        var normalizedSkills = NormalizeSkills(request.Skills);

        await _unitOfWork.UserSkills.ReplaceUserSkillsAsync(userId, normalizedSkills, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await BuildResultAsync(careerPathId, normalizedSkills, cancellationToken);
    }

    public async Task<SkillGapResultDto> GetMyResultAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var careerPathId = await ResolveCareerPathIdAsync(userId, null, cancellationToken);
        var userSkills = await _unitOfWork.UserSkills.GetByUserIdAsync(userId, cancellationToken);
        var normalizedSkills = NormalizeSkills(userSkills.Select(userSkill => userSkill.SkillName));

        return await BuildResultAsync(careerPathId, normalizedSkills, cancellationToken);
    }

    private async Task<int> ResolveCareerPathIdAsync(
        Guid userId,
        int? requestedCareerPathId,
        CancellationToken cancellationToken)
    {
        if (requestedCareerPathId.HasValue)
        {
            var careerPath = await _unitOfWork.CareerPaths.GetByIdAsync(requestedCareerPathId.Value, cancellationToken);

            if (careerPath is null)
            {
                throw new KeyNotFoundException("Career path was not found.");
            }

            return requestedCareerPathId.Value;
        }

        var selectedCareerPath = await _unitOfWork.UserCareerPaths.GetByUserIdAsync(userId, cancellationToken);

        if (selectedCareerPath is null)
        {
            throw new InvalidOperationException("No career path has been selected.");
        }

        return selectedCareerPath.CareerPathId;
    }

    private async Task<SkillGapResultDto> BuildResultAsync(
        int careerPathId,
        IReadOnlySet<string> normalizedSkills,
        CancellationToken cancellationToken)
    {
        var careerPath = await _unitOfWork.CareerPaths.GetByIdAsync(careerPathId, cancellationToken);

        if (careerPath is null)
        {
            throw new KeyNotFoundException("Career path was not found.");
        }

        var requiredSkillNodes = await _unitOfWork.SkillNodes.GetByCareerPathIdAsync(careerPathId, cancellationToken);
        var matchedSkills = new List<SkillGapSkillDto>();
        var missingSkills = new List<SkillGapSkillDto>();

        foreach (var skillNode in requiredSkillNodes.OrderBy(skillNode => skillNode.DisplayOrder))
        {
            var skillDto = MapSkill(skillNode);
            var normalizedRequiredSkill = NormalizeSkill(skillNode.Name);

            if (normalizedSkills.Contains(normalizedRequiredSkill))
            {
                matchedSkills.Add(skillDto);
            }
            else
            {
                missingSkills.Add(skillDto);
            }
        }

        var totalRequiredSkills = requiredSkillNodes.Count;

        return new SkillGapResultDto
        {
            CareerPathId = careerPath.CareerPathId,
            CareerPathName = careerPath.Name,
            TotalRequiredSkills = totalRequiredSkills,
            MatchedSkillCount = matchedSkills.Count,
            MissingSkillCount = missingSkills.Count,
            MatchPercentage = totalRequiredSkills == 0
                ? 0
                : Math.Round((decimal)matchedSkills.Count / totalRequiredSkills * 100, 2),
            MatchedSkills = matchedSkills,
            MissingSkills = missingSkills,
            RecommendedLearningPriority = missingSkills
        };
    }

    private static IReadOnlySet<string> NormalizeSkills(IEnumerable<string> skills)
    {
        return skills
            .Where(skill => !string.IsNullOrWhiteSpace(skill))
            .Select(NormalizeSkill)
            .Where(skill => skill.Length > 0)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    private static string NormalizeSkill(string skill)
    {
        return skill.Trim().ToLowerInvariant();
    }

    private static SkillGapSkillDto MapSkill(SkillNode skillNode)
    {
        return new SkillGapSkillDto
        {
            SkillNodeId = skillNode.SkillNodeId,
            Name = skillNode.Name,
            Description = skillNode.Description,
            Difficulty = skillNode.Difficulty.ToString(),
            DisplayOrder = skillNode.DisplayOrder,
            EstimatedHours = skillNode.EstimatedHours
        };
    }
}
