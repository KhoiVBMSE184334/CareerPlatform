using CareerPlatform.Application.DTOs.Roadmaps;
using CareerPlatform.Application.DTOs.SkillNodes;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Services;

public class SkillNodeService : ISkillNodeService
{
    private readonly IUnitOfWork _unitOfWork;

    public SkillNodeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<AdminSkillNodeDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skillNodes = await _unitOfWork.SkillNodes.GetAllAsync(cancellationToken);
        return skillNodes.Select(MapSkillNode).ToList();
    }

    public async Task<AdminSkillNodeDto?> GetByIdAsync(
        int skillNodeId,
        CancellationToken cancellationToken = default)
    {
        var skillNode = await _unitOfWork.SkillNodes.GetByIdAsync(skillNodeId, cancellationToken);
        return skillNode is null ? null : MapSkillNode(skillNode);
    }

    private static AdminSkillNodeDto MapSkillNode(SkillNode skillNode)
    {
        return new AdminSkillNodeDto
        {
            SkillNodeId = skillNode.SkillNodeId,
            CareerPathId = skillNode.CareerPathId,
            CareerPathName = skillNode.CareerPath.Name,
            Name = skillNode.Name,
            Description = skillNode.Description,
            Difficulty = skillNode.Difficulty.ToString(),
            DisplayOrder = skillNode.DisplayOrder,
            EstimatedHours = skillNode.EstimatedHours,
            LearningResources = skillNode.LearningResources
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
