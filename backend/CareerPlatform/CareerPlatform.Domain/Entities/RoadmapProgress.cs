namespace CareerPlatform.Domain.Entities;

public class RoadmapProgress
{
    public int ProgressId { get; set; }

    public Guid UserId { get; set; }

    public int SkillNodeId { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }

    public User User { get; set; } = null!;

    public SkillNode SkillNode { get; set; } = null!;
}
