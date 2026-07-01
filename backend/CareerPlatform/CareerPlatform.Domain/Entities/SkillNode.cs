using CareerPlatform.Domain.Enums;

namespace CareerPlatform.Domain.Entities;

public class SkillNode
{
    public int SkillNodeId { get; set; }

    public int CareerPathId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Difficulty Difficulty { get; set; }

    public int DisplayOrder { get; set; }

    public int? EstimatedHours { get; set; }

    public CareerPath CareerPath { get; set; } = null!;

    public ICollection<LearningResource> LearningResources { get; set; } = new List<LearningResource>();

    public ICollection<RoadmapProgress> RoadmapProgresses { get; set; } = new List<RoadmapProgress>();
}
