namespace CareerPlatform.Application.DTOs.Roadmaps;

public class RoadmapSkillNodeDto
{
    public int SkillNodeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Difficulty { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public int? EstimatedHours { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }

    public IReadOnlyList<LearningResourceDto> LearningResources { get; set; } = [];
}
