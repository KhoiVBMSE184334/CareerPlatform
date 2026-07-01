using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Interfaces.Repositories;

public interface IPortfolioRepository
{
    Task<IReadOnlyList<PortfolioProject>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<PortfolioProject?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PortfolioProject>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<PortfolioProject?> GetByIdAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<PortfolioProject?> GetByGithubUrlAsync(Guid userId, string githubUrl, CancellationToken cancellationToken = default);

    Task AddAsync(PortfolioProject portfolioProject, CancellationToken cancellationToken = default);

    void Update(PortfolioProject portfolioProject);

    void Delete(PortfolioProject portfolioProject);
}
