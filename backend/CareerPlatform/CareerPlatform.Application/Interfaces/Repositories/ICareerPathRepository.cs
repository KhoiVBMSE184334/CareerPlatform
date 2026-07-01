using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Interfaces.Repositories;

public interface ICareerPathRepository
{
    Task<IReadOnlyList<CareerPath>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CareerPath?> GetByIdAsync(int careerPathId, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(string name, int? excludedCareerPathId = null, CancellationToken cancellationToken = default);

    Task AddAsync(CareerPath careerPath, CancellationToken cancellationToken = default);

    void Update(CareerPath careerPath);

    void Delete(CareerPath careerPath);
}
