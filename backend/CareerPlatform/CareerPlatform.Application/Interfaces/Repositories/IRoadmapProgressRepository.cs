using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Interfaces.Repositories;

public interface IRoadmapProgressRepository
{
    Task<IReadOnlyList<RoadmapProgress>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RoadmapProgress?> GetByUserAndSkillNodeAsync(Guid userId, int skillNodeId, CancellationToken cancellationToken = default);

    Task AddAsync(RoadmapProgress roadmapProgress, CancellationToken cancellationToken = default);

    void Update(RoadmapProgress roadmapProgress);
}
