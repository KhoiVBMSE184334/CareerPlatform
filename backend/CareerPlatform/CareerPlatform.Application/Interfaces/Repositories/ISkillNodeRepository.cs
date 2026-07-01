using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Interfaces.Repositories;

public interface ISkillNodeRepository
{
    Task<IReadOnlyList<SkillNode>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<SkillNode?> GetByIdAsync(int skillNodeId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SkillNode>> GetByCareerPathIdAsync(int careerPathId, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task AddAsync(SkillNode skillNode, CancellationToken cancellationToken = default);

    void Update(SkillNode skillNode);

    void Delete(SkillNode skillNode);
}
