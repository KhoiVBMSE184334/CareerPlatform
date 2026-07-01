using CareerPlatform.Application.DTOs.CareerPaths;
using CareerPlatform.Application.DTOs.Chats;

namespace CareerPlatform.Application.DTOs.Dashboards;

public class StudentDashboardDto
{
    public CareerPathDto? SelectedCareerPath { get; set; }

    public decimal RoadmapCompletionPercentage { get; set; }

    public decimal SkillMatchPercentage { get; set; }

    public IReadOnlyList<string> MissingSkillsSummary { get; set; } = [];

    public IReadOnlyList<ChatSessionDto> RecentAIChatSessions { get; set; } = [];

    public int PortfolioProjectCount { get; set; }
}
