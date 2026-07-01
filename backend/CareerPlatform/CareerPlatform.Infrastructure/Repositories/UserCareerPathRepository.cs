using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class UserCareerPathRepository : IUserCareerPathRepository
{
    private readonly AppDbContext _context;

    public UserCareerPathRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<UserCareerPath?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.UserCareerPaths
            .Include(userCareerPath => userCareerPath.CareerPath)
            .FirstOrDefaultAsync(userCareerPath => userCareerPath.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(UserCareerPath userCareerPath, CancellationToken cancellationToken = default)
    {
        await _context.UserCareerPaths.AddAsync(userCareerPath, cancellationToken);
    }

    public void Update(UserCareerPath userCareerPath)
    {
        _context.UserCareerPaths.Update(userCareerPath);
    }
}
