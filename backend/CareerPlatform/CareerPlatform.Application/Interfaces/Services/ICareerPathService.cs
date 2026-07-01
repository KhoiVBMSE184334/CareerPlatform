using CareerPlatform.Application.DTOs.CareerPaths;

namespace CareerPlatform.Application.Interfaces.Services;

public interface ICareerPathService
{
    Task<IReadOnlyList<CareerPathDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CareerPathDto?> GetByIdAsync(int careerPathId, CancellationToken cancellationToken = default);

    Task<CareerPathDto> CreateAsync(CreateCareerPathDto request, CancellationToken cancellationToken = default);

    Task<CareerPathDto> UpdateAsync(int careerPathId, UpdateCareerPathDto request, CancellationToken cancellationToken = default);

    Task DeleteAsync(int careerPathId, CancellationToken cancellationToken = default);

    Task<CareerPathDto> SelectAsync(Guid userId, int careerPathId, CancellationToken cancellationToken = default);
}
