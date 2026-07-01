using CareerPlatform.Application.DTOs.Users;

namespace CareerPlatform.Application.Interfaces.Services;

public interface IUserService
{
    Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid userId, Guid currentUserId, CancellationToken cancellationToken = default);
}
