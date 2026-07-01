using CareerPlatform.Application.DTOs.Dashboards;

namespace CareerPlatform.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<StudentDashboardDto> GetStudentDashboardAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<AdminDashboardDto> GetAdminDashboardAsync(CancellationToken cancellationToken = default);
}
