namespace CareerPlatform.Application.DTOs.SkillGaps;

public class SkillGapResultDto
{
    public int CareerPathId { get; set; }

    public string CareerPathName { get; set; } = string.Empty;

    public int TotalRequiredSkills { get; set; }

    public int MatchedSkillCount { get; set; }

    public int MissingSkillCount { get; set; }

    public decimal MatchPercentage { get; set; }

    public IReadOnlyList<SkillGapSkillDto> MatchedSkills { get; set; } = [];

    public IReadOnlyList<SkillGapSkillDto> MissingSkills { get; set; } = [];

    public IReadOnlyList<SkillGapSkillDto> RecommendedLearningPriority { get; set; } = [];
}
