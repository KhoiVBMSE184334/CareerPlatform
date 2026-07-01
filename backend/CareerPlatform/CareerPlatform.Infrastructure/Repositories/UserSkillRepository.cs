using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class UserSkillRepository : IUserSkillRepository
{
    private readonly AppDbContext _context;

    public UserSkillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UserSkill>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserSkills
            .Where(userSkill => userSkill.UserId == userId)
            .OrderBy(userSkill => userSkill.SkillName)
            .ToListAsync(cancellationToken);
    }

    public async Task ReplaceUserSkillsAsync(
        Guid userId,
        IEnumerable<string> skillNames,
        CancellationToken cancellationToken = default)
    {
        var existingSkills = await _context.UserSkills
            .Where(userSkill => userSkill.UserId == userId)
            .ToListAsync(cancellationToken);

        _context.UserSkills.RemoveRange(existingSkills);

        var newSkills = skillNames
            .Where(skillName => !string.IsNullOrWhiteSpace(skillName))
            .Select(skillName => skillName.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(skillName => new UserSkill
            {
                UserId = userId,
                SkillName = skillName
            });

        await _context.UserSkills.AddRangeAsync(newSkills, cancellationToken);
    }
}
