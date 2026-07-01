using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class SkillNodeRepository : ISkillNodeRepository
{
    private readonly AppDbContext _context;

    public SkillNodeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<SkillNode>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SkillNodes
            .Include(skillNode => skillNode.CareerPath)
            .Include(skillNode => skillNode.LearningResources)
            .OrderBy(skillNode => skillNode.CareerPath.Name)
            .ThenBy(skillNode => skillNode.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public Task<SkillNode?> GetByIdAsync(int skillNodeId, CancellationToken cancellationToken = default)
    {
        return _context.SkillNodes
            .Include(skillNode => skillNode.CareerPath)
            .Include(skillNode => skillNode.LearningResources)
            .FirstOrDefaultAsync(skillNode => skillNode.SkillNodeId == skillNodeId, cancellationToken);
    }

    public async Task<IReadOnlyList<SkillNode>> GetByCareerPathIdAsync(int careerPathId, CancellationToken cancellationToken = default)
    {
        return await _context.SkillNodes
            .Include(skillNode => skillNode.LearningResources)
            .Where(skillNode => skillNode.CareerPathId == careerPathId)
            .OrderBy(skillNode => skillNode.DisplayOrder)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _context.SkillNodes.CountAsync(cancellationToken);
    }

    public async Task AddAsync(SkillNode skillNode, CancellationToken cancellationToken = default)
    {
        await _context.SkillNodes.AddAsync(skillNode, cancellationToken);
    }

    public void Update(SkillNode skillNode)
    {
        _context.SkillNodes.Update(skillNode);
    }

    public void Delete(SkillNode skillNode)
    {
        _context.SkillNodes.Remove(skillNode);
    }
}
