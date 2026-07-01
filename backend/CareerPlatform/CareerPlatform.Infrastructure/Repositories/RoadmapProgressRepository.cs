using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class RoadmapProgressRepository : IRoadmapProgressRepository
{
    private readonly AppDbContext _context;

    public RoadmapProgressRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<RoadmapProgress>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.RoadmapProgresses
            .Where(progress => progress.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public Task<RoadmapProgress?> GetByUserAndSkillNodeAsync(Guid userId, int skillNodeId, CancellationToken cancellationToken = default)
    {
        return _context.RoadmapProgresses
            .FirstOrDefaultAsync(
                progress => progress.UserId == userId && progress.SkillNodeId == skillNodeId,
                cancellationToken);
    }

    public async Task AddAsync(RoadmapProgress roadmapProgress, CancellationToken cancellationToken = default)
    {
        await _context.RoadmapProgresses.AddAsync(roadmapProgress, cancellationToken);
    }

    public void Update(RoadmapProgress roadmapProgress)
    {
        _context.RoadmapProgresses.Update(roadmapProgress);
    }
}
