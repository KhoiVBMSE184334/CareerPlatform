using CareerPlatform.Application.DTOs.Portfolios;

namespace CareerPlatform.Application.Interfaces.Services;

public interface IGitHubService
{
    Task<IReadOnlyList<GitHubRepoDto>> GetPublicRepositoriesAsync(
        string githubUrlOrUsername,
        CancellationToken cancellationToken = default);
}
