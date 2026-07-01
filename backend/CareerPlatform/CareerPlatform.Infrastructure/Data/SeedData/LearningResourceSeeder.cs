namespace CareerPlatform.Infrastructure.Data.SeedData;

public static class LearningResourceSeeder
{
    public static object[] LearningResources =>
    [
        new { ResourceId = 1, SkillNodeId = 1, Title = "Microsoft Learn C#", Url = "https://learn.microsoft.com/dotnet/csharp/" },
        new { ResourceId = 2, SkillNodeId = 1, Title = "FreeCodeCamp C# Tutorial", Url = "https://www.freecodecamp.org/news/tag/c-sharp/" },
        new { ResourceId = 3, SkillNodeId = 2, Title = "Microsoft OOP Concepts", Url = "https://learn.microsoft.com/dotnet/csharp/fundamentals/tutorials/oop" },
        new { ResourceId = 4, SkillNodeId = 2, Title = "Object Oriented Programming in C#", Url = "https://learn.microsoft.com/dotnet/csharp/fundamentals/object-oriented/" },
        new { ResourceId = 5, SkillNodeId = 3, Title = "Microsoft ASP.NET Core Documentation", Url = "https://learn.microsoft.com/aspnet/core/" },
        new { ResourceId = 6, SkillNodeId = 3, Title = "ASP.NET Core Web API Tutorial", Url = "https://learn.microsoft.com/aspnet/core/tutorials/first-web-api" },
        new { ResourceId = 7, SkillNodeId = 4, Title = "SQL Server Documentation", Url = "https://learn.microsoft.com/sql/sql-server/" },
        new { ResourceId = 8, SkillNodeId = 4, Title = "SQL Server Tutorial", Url = "https://learn.microsoft.com/sql/t-sql/tutorial-writing-transact-sql-statements" },
        new { ResourceId = 9, SkillNodeId = 5, Title = "REST API Design Guidance", Url = "https://learn.microsoft.com/azure/architecture/best-practices/api-design" },
        new { ResourceId = 10, SkillNodeId = 5, Title = "HTTP Methods and Status Codes", Url = "https://developer.mozilla.org/docs/Web/HTTP" },
        new { ResourceId = 11, SkillNodeId = 6, Title = "JWT Authentication in ASP.NET Core", Url = "https://learn.microsoft.com/aspnet/core/security/authentication/" },
        new { ResourceId = 12, SkillNodeId = 6, Title = "JSON Web Tokens Introduction", Url = "https://jwt.io/introduction" },
        new { ResourceId = 13, SkillNodeId = 7, Title = "Docker Get Started", Url = "https://docs.docker.com/get-started/" },
        new { ResourceId = 14, SkillNodeId = 7, Title = "Docker for .NET Developers", Url = "https://learn.microsoft.com/dotnet/architecture/microservices/container-docker-introduction/" },
        new { ResourceId = 15, SkillNodeId = 8, Title = "MDN HTML Guide", Url = "https://developer.mozilla.org/docs/Web/HTML" },
        new { ResourceId = 16, SkillNodeId = 8, Title = "FreeCodeCamp HTML Tutorial", Url = "https://www.freecodecamp.org/news/tag/html/" },
        new { ResourceId = 17, SkillNodeId = 9, Title = "MDN CSS Guide", Url = "https://developer.mozilla.org/docs/Web/CSS" },
        new { ResourceId = 18, SkillNodeId = 9, Title = "Web.dev Learn CSS", Url = "https://web.dev/learn/css" },
        new { ResourceId = 19, SkillNodeId = 10, Title = "MDN JavaScript Guide", Url = "https://developer.mozilla.org/docs/Web/JavaScript/Guide" },
        new { ResourceId = 20, SkillNodeId = 10, Title = "JavaScript.info", Url = "https://javascript.info/" },
        new { ResourceId = 21, SkillNodeId = 11, Title = "TypeScript Handbook", Url = "https://www.typescriptlang.org/docs/handbook/intro.html" },
        new { ResourceId = 22, SkillNodeId = 11, Title = "TypeScript for JavaScript Programmers", Url = "https://www.typescriptlang.org/docs/handbook/typescript-in-5-minutes.html" },
        new { ResourceId = 23, SkillNodeId = 12, Title = "React Official Documentation", Url = "https://react.dev/" },
        new { ResourceId = 24, SkillNodeId = 12, Title = "React Crash Course", Url = "https://www.freecodecamp.org/news/react-crash-course/" },
        new { ResourceId = 25, SkillNodeId = 13, Title = "React Router Documentation", Url = "https://reactrouter.com/" },
        new { ResourceId = 26, SkillNodeId = 13, Title = "React Router Tutorial", Url = "https://reactrouter.com/en/main/start/tutorial" },
        new { ResourceId = 27, SkillNodeId = 14, Title = "Axios Documentation", Url = "https://axios-http.com/docs/intro" },
        new { ResourceId = 28, SkillNodeId = 14, Title = "Fetch API Guide", Url = "https://developer.mozilla.org/docs/Web/API/Fetch_API/Using_Fetch" }
    ];
}
