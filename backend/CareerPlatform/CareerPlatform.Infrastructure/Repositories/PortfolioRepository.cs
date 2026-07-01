using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly AppDbContext _context;

    public PortfolioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<PortfolioProject>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PortfolioProjects
            .Include(project => project.User)
            .OrderByDescending(project => project.ImportedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<PortfolioProject?> GetByIdAsync(Guid projectId, CancellationToken cancellationToken = default)
    {
        return _context.PortfolioProjects
            .Include(project => project.User)
            .FirstOrDefaultAsync(project => project.ProjectId == projectId, cancellationToken);
    }

    public async Task<IReadOnlyList<PortfolioProject>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.PortfolioProjects
            .Where(project => project.UserId == userId)
            .OrderByDescending(project => project.ImportedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<PortfolioProject?> GetByIdAsync(Guid projectId, Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.PortfolioProjects
            .FirstOrDefaultAsync(
                project => project.ProjectId == projectId && project.UserId == userId,
                cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _context.PortfolioProjects.CountAsync(cancellationToken);
    }

    public Task<PortfolioProject?> GetByGithubUrlAsync(Guid userId, string githubUrl, CancellationToken cancellationToken = default)
    {
        return _context.PortfolioProjects
            .FirstOrDefaultAsync(
                project => project.UserId == userId && project.GithubUrl == githubUrl,
                cancellationToken);
    }

    public async Task AddAsync(PortfolioProject portfolioProject, CancellationToken cancellationToken = default)
    {
        await _context.PortfolioProjects.AddAsync(portfolioProject, cancellationToken);
    }

    public void Update(PortfolioProject portfolioProject)
    {
        _context.PortfolioProjects.Update(portfolioProject);
    }

    public void Delete(PortfolioProject portfolioProject)
    {
        _context.PortfolioProjects.Remove(portfolioProject);
    }
}
