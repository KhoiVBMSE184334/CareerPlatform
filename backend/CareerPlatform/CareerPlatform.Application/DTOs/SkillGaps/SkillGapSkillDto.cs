namespace CareerPlatform.Application.DTOs.SkillGaps;

public class SkillGapSkillDto
{
    public int SkillNodeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string Difficulty { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }

    public int? EstimatedHours { get; set; }
}
