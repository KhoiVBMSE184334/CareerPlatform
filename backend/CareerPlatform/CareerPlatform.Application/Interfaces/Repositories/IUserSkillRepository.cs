using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Interfaces.Repositories;

public interface IUserSkillRepository
{
    Task<IReadOnlyList<UserSkill>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task ReplaceUserSkillsAsync(Guid userId, IEnumerable<string> skillNames, CancellationToken cancellationToken = default);
}
