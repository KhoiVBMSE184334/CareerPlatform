using CareerPlatform.Application.Interfaces.Repositories;

namespace CareerPlatform.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }

    IUserSkillRepository UserSkills { get; }

    ICareerPathRepository CareerPaths { get; }

    ISkillNodeRepository SkillNodes { get; }

    IUserCareerPathRepository UserCareerPaths { get; }

    IRoadmapProgressRepository RoadmapProgresses { get; }

    IChatRepository Chats { get; }

    IPortfolioRepository PortfolioProjects { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
