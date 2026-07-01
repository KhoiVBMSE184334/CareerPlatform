using CareerPlatform.Application.DTOs.Roadmaps;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Services;

public class RoadmapService : IRoadmapService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoadmapService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RoadmapDto> GetSelectedRoadmapAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var selectedCareerPath = await _unitOfWork.UserCareerPaths.GetByUserIdAsync(userId, cancellationToken);

        if (selectedCareerPath is null)
        {
            throw new InvalidOperationException("No career path has been selected.");
        }

        return await BuildRoadmapAsync(userId, selectedCareerPath.CareerPathId, cancellationToken);
    }

    public Task<RoadmapDto> GetRoadmapByCareerPathAsync(Guid userId, int careerPathId, CancellationToken cancellationToken = default)
    {
        return BuildRoadmapAsync(userId, careerPathId, cancellationToken);
    }

    public async Task<RoadmapDto> UpdateProgressAsync(Guid userId, ProgressUpdateDto request, CancellationToken cancellationToken = default)
    {
        var selectedCareerPath = await _unitOfWork.UserCareerPaths.GetByUserIdAsync(userId, cancellationToken);

        if (selectedCareerPath is null)
        {
            throw new InvalidOperationException("No career path has been selected.");
        }

        var skillNode = await _unitOfWork.SkillNodes.GetByIdAsync(request.SkillNodeId, cancellationToken);

        if (skillNode is null)
        {
            throw new KeyNotFoundException("Skill node was not found.");
        }

        if (skillNode.CareerPathId != selectedCareerPath.CareerPathId)
        {
            throw new InvalidOperationException("Skill node does not belong to the selected career path.");
        }

        var progress = await _unitOfWork.RoadmapProgresses.GetByUserAndSkillNodeAsync(
            userId,
            request.SkillNodeId,
            cancellationToken);

        if (progress is null)
        {
            progress = new RoadmapProgress
            {
                UserId = userId,
                SkillNodeId = request.SkillNodeId,
                IsCompleted = request.IsCompleted,
                CompletedAt = request.IsCompleted ? DateTime.UtcNow : null
            };

            await _unitOfWork.RoadmapProgresses.AddAsync(progress, cancellationToken);
        }
        else
        {
            progress.IsCompleted = request.IsCompleted;
            progress.CompletedAt = request.IsCompleted ? DateTime.UtcNow : null;
            _unitOfWork.RoadmapProgresses.Update(progress);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await BuildRoadmapAsync(userId, selectedCareerPath.CareerPathId, cancellationToken);
    }

    private async Task<RoadmapDto> BuildRoadmapAsync(Guid userId, int careerPathId, CancellationToken cancellationToken)
    {
        var careerPath = await _unitOfWork.CareerPaths.GetByIdAsync(careerPathId, cancellationToken);

        if (careerPath is null)
        {
            throw new KeyNotFoundException("Career path was not found.");
        }

        var skillNodes = await _unitOfWork.SkillNodes.GetByCareerPathIdAsync(careerPathId, cancellationToken);
        var progressRecords = await _unitOfWork.RoadmapProgresses.GetByUserIdAsync(userId, cancellationToken);
        var progressBySkillNodeId = progressRecords.ToDictionary(progress => progress.SkillNodeId);

        var roadmapSkillNodes = skillNodes
            .Select(skillNode => MapSkillNode(skillNode, progressBySkillNodeId))
            .ToList();

        var completedSkillNodes = roadmapSkillNodes.Count(skillNode => skillNode.IsCompleted);
        var totalSkillNodes = roadmapSkillNodes.Count;

        return new RoadmapDto
        {
            CareerPathId = careerPath.CareerPathId,
            CareerPathName = careerPath.Name,
            CareerPathDescription = careerPath.Description,
            TotalSkillNodes = totalSkillNodes,
            CompletedSkillNodes = completedSkillNodes,
            CompletionPercentage = totalSkillNodes == 0
                ? 0
                : Math.Round((decimal)completedSkillNodes / totalSkillNodes * 100, 2),
            SkillNodes = roadmapSkillNodes
        };
    }

    private static RoadmapSkillNodeDto MapSkillNode(
        SkillNode skillNode,
        IReadOnlyDictionary<int, RoadmapProgress> progressBySkillNodeId)
    {
        progressBySkillNodeId.TryGetValue(skillNode.SkillNodeId, out var progress);

        return new RoadmapSkillNodeDto
        {
            SkillNodeId = skillNode.SkillNodeId,
            Name = skillNode.Name,
            Description = skillNode.Description,
            Difficulty = skillNode.Difficulty.ToString(),
            DisplayOrder = skillNode.DisplayOrder,
            EstimatedHours = skillNode.EstimatedHours,
            IsCompleted = progress?.IsCompleted ?? false,
            CompletedAt = progress?.CompletedAt,
            LearningResources = skillNode.LearningResources
                .OrderBy(resource => resource.ResourceId)
                .Select(resource => new LearningResourceDto
                {
                    ResourceId = resource.ResourceId,
                    Title = resource.Title,
                    Url = resource.Url
                })
                .ToList()
        };
    }
}
