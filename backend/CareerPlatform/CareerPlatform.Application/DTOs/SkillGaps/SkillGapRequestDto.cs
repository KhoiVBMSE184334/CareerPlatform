namespace CareerPlatform.Application.DTOs.SkillGaps;

public class SkillGapRequestDto
{
    public int? CareerPathId { get; set; }

    public IReadOnlyList<string> Skills { get; set; } = [];
}
