using CareerPlatform.Application.DTOs.Portfolios;

namespace CareerPlatform.Application.Interfaces.Services;

public interface IPortfolioService
{
    Task<IReadOnlyList<AdminPortfolioProjectDto>> GetAllForAdminAsync(CancellationToken cancellationToken = default);

    Task<AdminPortfolioProjectDto?> GetByIdForAdminAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PortfolioProjectDto>> ImportFromGitHubAsync(
        Guid userId,
        ImportGitHubRequestDto request,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PortfolioProjectDto>> GetMyPortfolioAsync(Guid userId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid userId, Guid projectId, CancellationToken cancellationToken = default);
}
