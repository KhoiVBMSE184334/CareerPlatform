using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using CareerPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .OrderBy(user => user.Role)
            .ThenBy(user => user.FullName)
            .ToListAsync(cancellationToken);
    }

    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .Include(user => user.StudentProfile)
            .Include(user => user.UserSkills)
            .Include(user => user.UserCareerPaths)
            .Include(user => user.RoadmapProgresses)
            .Include(user => user.ChatSessions)
            .Include(user => user.PortfolioProjects)
            .FirstOrDefaultAsync(user => user.UserId == userId, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _context.Users
            .Include(user => user.StudentProfile)
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _context.Users.CountAsync(cancellationToken);
    }

    public Task<int> CountByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        return _context.Users.CountAsync(user => user.Role == role, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }
}
