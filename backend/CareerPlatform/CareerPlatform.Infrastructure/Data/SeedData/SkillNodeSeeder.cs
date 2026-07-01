using CareerPlatform.Domain.Enums;

namespace CareerPlatform.Infrastructure.Data.SeedData;

public static class SkillNodeSeeder
{
    public static object[] SkillNodes =>
    [
        new
        {
            SkillNodeId = 1,
            CareerPathId = 1,
            Name = "C#",
            Description = "Learn C# syntax, types, collections, and language fundamentals.",
            Difficulty = Difficulty.Beginner,
            DisplayOrder = 1,
            EstimatedHours = 20
        },
        new
        {
            SkillNodeId = 2,
            CareerPathId = 1,
            Name = "Object Oriented Programming",
            Description = "Understand classes, interfaces, inheritance, encapsulation, and polymorphism.",
            Difficulty = Difficulty.Beginner,
            DisplayOrder = 2,
            EstimatedHours = 16
        },
        new
        {
            SkillNodeId = 3,
            CareerPathId = 1,
            Name = "ASP.NET Core",
            Description = "Build backend applications and APIs with ASP.NET Core.",
            Difficulty = Difficulty.Intermediate,
            DisplayOrder = 3,
            EstimatedHours = 24
        },
        new
        {
            SkillNodeId = 4,
            CareerPathId = 1,
            Name = "SQL Server",
            Description = "Design relational schemas and query data with SQL Server.",
            Difficulty = Difficulty.Beginner,
            DisplayOrder = 4,
            EstimatedHours = 18
        },
        new
        {
            SkillNodeId = 5,
            CareerPathId = 1,
            Name = "REST API",
            Description = "Design RESTful endpoints, status codes, validation, and error responses.",
            Difficulty = Difficulty.Intermediate,
            DisplayOrder = 5,
            EstimatedHours = 16
        },
        new
        {
            SkillNodeId = 6,
            CareerPathId = 1,
            Name = "JWT Authentication",
            Description = "Implement token-based authentication and role-based authorization.",
            Difficulty = Difficulty.Intermediate,
            DisplayOrder = 6,
            EstimatedHours = 16
        },
        new
        {
            SkillNodeId = 7,
            CareerPathId = 1,
            Name = "Docker Basics",
            Description = "Learn container fundamentals for packaging and running backend applications.",
            Difficulty = Difficulty.Beginner,
            DisplayOrder = 7,
            EstimatedHours = 12
        },
        new
        {
            SkillNodeId = 8,
            CareerPathId = 2,
            Name = "HTML",
            Description = "Structure web pages with semantic HTML.",
            Difficulty = Difficulty.Beginner,
            DisplayOrder = 1,
            EstimatedHours = 10
        },
        new
        {
            SkillNodeId = 9,
            CareerPathId = 2,
            Name = "CSS",
            Description = "Style responsive layouts with CSS fundamentals.",
            Difficulty = Difficulty.Beginner,
            DisplayOrder = 2,
            EstimatedHours = 14
        },
        new
        {
            SkillNodeId = 10,
            CareerPathId = 2,
            Name = "JavaScript",
            Description = "Learn JavaScript syntax, DOM interaction, and asynchronous programming.",
            Difficulty = Difficulty.Beginner,
            DisplayOrder = 3,
            EstimatedHours = 22
        },
        new
        {
            SkillNodeId = 11,
            CareerPathId = 2,
            Name = "TypeScript",
            Description = "Add static typing and safer structure to JavaScript applications.",
            Difficulty = Difficulty.Intermediate,
            DisplayOrder = 4,
            EstimatedHours = 16
        },
        new
        {
            SkillNodeId = 12,
            CareerPathId = 2,
            Name = "React",
            Description = "Build component-based user interfaces with React.",
            Difficulty = Difficulty.Intermediate,
            DisplayOrder = 5,
            EstimatedHours = 24
        },
        new
        {
            SkillNodeId = 13,
            CareerPathId = 2,
            Name = "React Router",
            Description = "Implement client-side routing and navigation in React apps.",
            Difficulty = Difficulty.Intermediate,
            DisplayOrder = 6,
            EstimatedHours = 10
        },
        new
        {
            SkillNodeId = 14,
            CareerPathId = 2,
            Name = "API Integration",
            Description = "Connect frontend applications to backend APIs using HTTP clients.",
            Difficulty = Difficulty.Intermediate,
            DisplayOrder = 7,
            EstimatedHours = 14
        }
    ];
}
