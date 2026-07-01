using CareerPlatform.Domain.Enums;

namespace CareerPlatform.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public StudentProfile? StudentProfile { get; set; }

    public ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();

    public ICollection<UserCareerPath> UserCareerPaths { get; set; } = new List<UserCareerPath>();

    public ICollection<RoadmapProgress> RoadmapProgresses { get; set; } = new List<RoadmapProgress>();

    public ICollection<ChatSession> ChatSessions { get; set; } = new List<ChatSession>();

    public ICollection<PortfolioProject> PortfolioProjects { get; set; } = new List<PortfolioProject>();
}
