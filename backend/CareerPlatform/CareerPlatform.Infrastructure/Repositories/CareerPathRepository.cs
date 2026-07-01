using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class CareerPathRepository : ICareerPathRepository
{
    private readonly AppDbContext _context;

    public CareerPathRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CareerPath>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CareerPaths
            .Include(careerPath => careerPath.SkillNodes)
            .OrderBy(careerPath => careerPath.CareerPathId)
            .ToListAsync(cancellationToken);
    }

    public Task<CareerPath?> GetByIdAsync(int careerPathId, CancellationToken cancellationToken = default)
    {
        return _context.CareerPaths
            .Include(careerPath => careerPath.SkillNodes)
            .FirstOrDefaultAsync(careerPath => careerPath.CareerPathId == careerPathId, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _context.CareerPaths.CountAsync(cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string name, int? excludedCareerPathId = null, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToLower();

        return _context.CareerPaths.AnyAsync(
            careerPath =>
                careerPath.Name.ToLower() == normalizedName
                && (!excludedCareerPathId.HasValue || careerPath.CareerPathId != excludedCareerPathId.Value),
            cancellationToken);
    }

    public async Task AddAsync(CareerPath careerPath, CancellationToken cancellationToken = default)
    {
        await _context.CareerPaths.AddAsync(careerPath, cancellationToken);
    }

    public void Update(CareerPath careerPath)
    {
        _context.CareerPaths.Update(careerPath);
    }

    public void Delete(CareerPath careerPath)
    {
        _context.CareerPaths.Remove(careerPath);
    }
}
