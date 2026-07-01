using CareerPlatform.Application.DTOs.SkillGaps;

namespace CareerPlatform.Application.Interfaces.Services;

public interface ISkillGapService
{
    Task<SkillGapResultDto> AnalyzeAsync(Guid userId, SkillGapRequestDto request, CancellationToken cancellationToken = default);

    Task<SkillGapResultDto> GetMyResultAsync(Guid userId, CancellationToken cancellationToken = default);
}
