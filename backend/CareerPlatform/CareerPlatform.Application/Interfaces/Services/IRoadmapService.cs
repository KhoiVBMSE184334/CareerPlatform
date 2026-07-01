using CareerPlatform.Application.DTOs.Roadmaps;

namespace CareerPlatform.Application.Interfaces.Services;

public interface IRoadmapService
{
    Task<RoadmapDto> GetSelectedRoadmapAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<RoadmapDto> GetRoadmapByCareerPathAsync(Guid userId, int careerPathId, CancellationToken cancellationToken = default);

    Task<RoadmapDto> UpdateProgressAsync(Guid userId, ProgressUpdateDto request, CancellationToken cancellationToken = default);
}
