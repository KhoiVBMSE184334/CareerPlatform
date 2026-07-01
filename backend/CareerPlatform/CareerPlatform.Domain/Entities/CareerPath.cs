namespace CareerPlatform.Domain.Entities;

public class CareerPath
{
    public int CareerPathId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<SkillNode> SkillNodes { get; set; } = new List<SkillNode>();

    public ICollection<UserCareerPath> UserCareerPaths { get; set; } = new List<UserCareerPath>();
}
