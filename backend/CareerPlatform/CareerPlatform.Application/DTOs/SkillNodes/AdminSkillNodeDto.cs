using CareerPlatform.Application.DTOs.Roadmaps;

namespace CareerPlatform.Application.DTOs.SkillNodes;

public class AdminSkillNodeDto
{
    public int SkillNodeId { get; set; }

    public int CareerPathId { get; set; }

    public string CareerPathName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Difficulty { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public int? EstimatedHours { get; set; }

    public IReadOnlyList<LearningResourceDto> LearningResources { get; set; } = [];
}
