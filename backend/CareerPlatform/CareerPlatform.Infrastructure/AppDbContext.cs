using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<StudentProfile> StudentProfiles => Set<StudentProfile>();

    public DbSet<CareerPath> CareerPaths => Set<CareerPath>();

    public DbSet<SkillNode> SkillNodes => Set<SkillNode>();

    public DbSet<LearningResource> LearningResources => Set<LearningResource>();

    public DbSet<UserSkill> UserSkills => Set<UserSkill>();

    public DbSet<UserCareerPath> UserCareerPaths => Set<UserCareerPath>();

    public DbSet<RoadmapProgress> RoadmapProgresses => Set<RoadmapProgress>();

    public DbSet<ChatSession> ChatSessions => Set<ChatSession>();

    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    public DbSet<PortfolioProject> PortfolioProjects => Set<PortfolioProject>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
