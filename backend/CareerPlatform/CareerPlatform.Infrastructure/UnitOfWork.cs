using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Infrastructure.Repositories;

namespace CareerPlatform.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IUserRepository? _users;
    private IUserSkillRepository? _userSkills;
    private ICareerPathRepository? _careerPaths;
    private ISkillNodeRepository? _skillNodes;
    private IUserCareerPathRepository? _userCareerPaths;
    private IRoadmapProgressRepository? _roadmapProgresses;
    private IChatRepository? _chats;
    private IPortfolioRepository? _portfolioProjects;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);

    public IUserSkillRepository UserSkills => _userSkills ??= new UserSkillRepository(_context);

    public ICareerPathRepository CareerPaths => _careerPaths ??= new CareerPathRepository(_context);

    public ISkillNodeRepository SkillNodes => _skillNodes ??= new SkillNodeRepository(_context);

    public IUserCareerPathRepository UserCareerPaths => _userCareerPaths ??= new UserCareerPathRepository(_context);

    public IRoadmapProgressRepository RoadmapProgresses => _roadmapProgresses ??= new RoadmapProgressRepository(_context);

    public IChatRepository Chats => _chats ??= new ChatRepository(_context);

    public IPortfolioRepository PortfolioProjects => _portfolioProjects ??= new PortfolioRepository(_context);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
