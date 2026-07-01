using CareerPlatform.Application.DTOs.CareerPaths;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Services;

public class CareerPathService : ICareerPathService
{
    private readonly IUnitOfWork _unitOfWork;

    public CareerPathService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<CareerPathDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var careerPaths = await _unitOfWork.CareerPaths.GetAllAsync(cancellationToken);
        return careerPaths.Select(MapToDto).ToList();
    }

    public async Task<CareerPathDto?> GetByIdAsync(int careerPathId, CancellationToken cancellationToken = default)
    {
        var careerPath = await _unitOfWork.CareerPaths.GetByIdAsync(careerPathId, cancellationToken);
        return careerPath is null ? null : MapToDto(careerPath);
    }

    public async Task<CareerPathDto> CreateAsync(CreateCareerPathDto request, CancellationToken cancellationToken = default)
    {
        var name = request.Name.Trim();

        if (await _unitOfWork.CareerPaths.ExistsByNameAsync(name, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException("Career path name already exists.");
        }

        var careerPath = new CareerPath
        {
            Name = name,
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim()
        };

        await _unitOfWork.CareerPaths.AddAsync(careerPath, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(careerPath);
    }

    public async Task<CareerPathDto> UpdateAsync(int careerPathId, UpdateCareerPathDto request, CancellationToken cancellationToken = default)
    {
        var careerPath = await _unitOfWork.CareerPaths.GetByIdAsync(careerPathId, cancellationToken);

        if (careerPath is null)
        {
            throw new KeyNotFoundException("Career path was not found.");
        }

        var name = request.Name.Trim();

        if (await _unitOfWork.CareerPaths.ExistsByNameAsync(name, careerPathId, cancellationToken))
        {
            throw new InvalidOperationException("Career path name already exists.");
        }

        careerPath.Name = name;
        careerPath.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();

        _unitOfWork.CareerPaths.Update(careerPath);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(careerPath);
    }

    public async Task DeleteAsync(int careerPathId, CancellationToken cancellationToken = default)
    {
        var careerPath = await _unitOfWork.CareerPaths.GetByIdAsync(careerPathId, cancellationToken);

        if (careerPath is null)
        {
            throw new KeyNotFoundException("Career path was not found.");
        }

        if (careerPath.SkillNodes.Count > 0)
        {
            throw new InvalidOperationException("Career path cannot be deleted while it has skill nodes.");
        }

        _unitOfWork.CareerPaths.Delete(careerPath);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<CareerPathDto> SelectAsync(Guid userId, int careerPathId, CancellationToken cancellationToken = default)
    {
        var careerPath = await _unitOfWork.CareerPaths.GetByIdAsync(careerPathId, cancellationToken);

        if (careerPath is null)
        {
            throw new KeyNotFoundException("Career path was not found.");
        }

        var selectedCareerPath = await _unitOfWork.UserCareerPaths.GetByUserIdAsync(userId, cancellationToken);

        if (selectedCareerPath is null)
        {
            selectedCareerPath = new UserCareerPath
            {
                UserId = userId,
                CareerPathId = careerPathId
            };

            await _unitOfWork.UserCareerPaths.AddAsync(selectedCareerPath, cancellationToken);
        }
        else
        {
            selectedCareerPath.CareerPathId = careerPathId;
            _unitOfWork.UserCareerPaths.Update(selectedCareerPath);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return MapToDto(careerPath);
    }

    private static CareerPathDto MapToDto(CareerPath careerPath)
    {
        return new CareerPathDto
        {
            CareerPathId = careerPath.CareerPathId,
            Name = careerPath.Name,
            Description = careerPath.Description
        };
    }
}
