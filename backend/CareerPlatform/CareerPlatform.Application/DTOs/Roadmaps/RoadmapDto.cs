namespace CareerPlatform.Application.DTOs.Roadmaps;

public class RoadmapDto
{
    public int CareerPathId { get; set; }

    public string CareerPathName { get; set; } = string.Empty;

    public string? CareerPathDescription { get; set; }

    public int TotalSkillNodes { get; set; }

    public int CompletedSkillNodes { get; set; }

    public decimal CompletionPercentage { get; set; }

    public IReadOnlyList<RoadmapSkillNodeDto> SkillNodes { get; set; } = [];
}
