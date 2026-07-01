namespace CareerPlatform.Infrastructure.Data.SeedData;

public static class TestDataSeeder
{
    public static readonly Guid DemoChatSessionId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid DemoChatMessageUserId = Guid.Parse("55555555-5555-5555-5555-555555555555");
    public static readonly Guid DemoChatMessageAssistantId = Guid.Parse("66666666-6666-6666-6666-666666666666");
    public static readonly Guid DemoPortfolioProjectId = Guid.Parse("77777777-7777-7777-7777-777777777777");
    public static readonly Guid DemoPortfolioProjectApiId = Guid.Parse("88888888-8888-8888-8888-888888888888");

    public static object[] UserSkills =>
    [
        new
        {
            UserSkillId = 1,
            UserId = UserSeeder.StudentUserId,
            SkillName = "C#"
        },
        new
        {
            UserSkillId = 2,
            UserId = UserSeeder.StudentUserId,
            SkillName = "ASP.NET Core"
        },
        new
        {
            UserSkillId = 3,
            UserId = UserSeeder.StudentUserId,
            SkillName = "SQL Server"
        },
        new
        {
            UserSkillId = 4,
            UserId = UserSeeder.StudentUserId,
            SkillName = "REST API"
        }
    ];

    public static object[] UserCareerPaths =>
    [
        new
        {
            Id = 1,
            UserId = UserSeeder.StudentUserId,
            CareerPathId = 1
        }
    ];

    public static object[] RoadmapProgresses =>
    [
        new
        {
            ProgressId = 1,
            UserId = UserSeeder.StudentUserId,
            SkillNodeId = 1,
            IsCompleted = true,
            CompletedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
        },
        new
        {
            ProgressId = 2,
            UserId = UserSeeder.StudentUserId,
            SkillNodeId = 3,
            IsCompleted = true,
            CompletedAt = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc)
        },
        new
        {
            ProgressId = 3,
            UserId = UserSeeder.StudentUserId,
            SkillNodeId = 4,
            IsCompleted = false,
            CompletedAt = (DateTime?)null
        }
    ];

    public static object[] ChatSessions =>
    [
        new
        {
            SessionId = DemoChatSessionId,
            UserId = UserSeeder.StudentUserId,
            CreatedAt = new DateTime(2026, 1, 4, 9, 0, 0, DateTimeKind.Utc)
        }
    ];

    public static object[] ChatMessages =>
    [
        new
        {
            MessageId = DemoChatMessageUserId,
            SessionId = DemoChatSessionId,
            Role = "User",
            Content = "How should I continue learning backend development?",
            CreatedAt = new DateTime(2026, 1, 4, 9, 1, 0, DateTimeKind.Utc)
        },
        new
        {
            MessageId = DemoChatMessageAssistantId,
            SessionId = DemoChatSessionId,
            Role = "Assistant",
            Content = "Focus next on Object Oriented Programming, JWT Authentication, and Docker Basics to round out your backend roadmap.",
            CreatedAt = new DateTime(2026, 1, 4, 9, 2, 0, DateTimeKind.Utc)
        }
    ];

    public static object[] PortfolioProjects =>
    [
        new
        {
            ProjectId = DemoPortfolioProjectId,
            UserId = UserSeeder.StudentUserId,
            RepositoryName = "CareerPlatform",
            Description = "A career orientation and learning roadmap platform built with ASP.NET Core and React.",
            TechStack = "ASP.NET Core, EF Core, SQL Server, React",
            GithubUrl = "https://github.com/demo/CareerPlatform",
            ImportedAt = new DateTime(2026, 1, 5, 0, 0, 0, DateTimeKind.Utc)
        },
        new
        {
            ProjectId = DemoPortfolioProjectApiId,
            UserId = UserSeeder.StudentUserId,
            RepositoryName = "BackendApiPractice",
            Description = "A practice backend API project with authentication, CRUD endpoints, and SQL Server persistence.",
            TechStack = "C#, ASP.NET Core, SQL Server",
            GithubUrl = "https://github.com/demo/BackendApiPractice",
            ImportedAt = new DateTime(2026, 1, 6, 0, 0, 0, DateTimeKind.Utc)
        }
    ];
}
