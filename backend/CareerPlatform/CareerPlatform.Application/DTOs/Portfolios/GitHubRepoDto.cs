namespace CareerPlatform.Application.DTOs.Portfolios;

public class GitHubRepoDto
{
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Language { get; set; }

    public string GithubUrl { get; set; } = string.Empty;

    public string? ReadmeContent { get; set; }
}
