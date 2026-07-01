namespace CareerPlatform.Domain.Entities;

public class LearningResource
{
    public int ResourceId { get; set; }

    public int SkillNodeId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public SkillNode SkillNode { get; set; } = null!;
}
