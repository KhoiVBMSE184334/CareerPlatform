using CareerPlatform.Application.DTOs.SkillNodes;

namespace CareerPlatform.Application.Interfaces.Services;

public interface ISkillNodeService
{
    Task<IReadOnlyList<AdminSkillNodeDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AdminSkillNodeDto?> GetByIdAsync(int skillNodeId, CancellationToken cancellationToken = default);
}
