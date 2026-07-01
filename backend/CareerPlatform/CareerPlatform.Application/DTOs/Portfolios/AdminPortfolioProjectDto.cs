namespace CareerPlatform.Application.DTOs.Portfolios;

public class AdminPortfolioProjectDto
{
    public Guid ProjectId { get; set; }

    public Guid UserId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string RepositoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? TechStack { get; set; }

    public string GithubUrl { get; set; } = string.Empty;

    public DateTime ImportedAt { get; set; }
}
