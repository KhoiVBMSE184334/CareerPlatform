namespace CareerPlatform.Application.DTOs.Portfolios;

public class PortfolioProjectDto
{
    public Guid ProjectId { get; set; }

    public string RepositoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? TechStack { get; set; }

    public string GithubUrl { get; set; } = string.Empty;

    public DateTime ImportedAt { get; set; }
}
