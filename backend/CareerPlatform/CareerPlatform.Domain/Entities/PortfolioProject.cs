namespace CareerPlatform.Domain.Entities;

public class PortfolioProject
{
    public Guid ProjectId { get; set; }

    public Guid UserId { get; set; }

    public string RepositoryName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? TechStack { get; set; }

    public string GithubUrl { get; set; } = string.Empty;

    public DateTime ImportedAt { get; set; }

    public User User { get; set; } = null!;
}
