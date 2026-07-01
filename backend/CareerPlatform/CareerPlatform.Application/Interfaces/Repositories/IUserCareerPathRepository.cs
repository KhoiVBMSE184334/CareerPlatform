using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Interfaces.Repositories;

public interface IUserCareerPathRepository
{
    Task<UserCareerPath?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddAsync(UserCareerPath userCareerPath, CancellationToken cancellationToken = default);

    void Update(UserCareerPath userCareerPath);
}
