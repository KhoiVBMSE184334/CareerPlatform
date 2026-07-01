using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CareerPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CareerPaths",
                columns: table => new
                {
                    CareerPathId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerPaths", x => x.CareerPathId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "SkillNodes",
                columns: table => new
                {
                    SkillNodeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CareerPathId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    EstimatedHours = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillNodes", x => x.SkillNodeId);
                    table.ForeignKey(
                        name: "FK_SkillNodes_CareerPaths_CareerPathId",
                        column: x => x.CareerPathId,
                        principalTable: "CareerPaths",
                        principalColumn: "CareerPathId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChatSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_ChatSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioProjects",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RepositoryName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TechStack = table.Column<string>(type: "text", nullable: true),
                    GithubUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ImportedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioProjects", x => x.ProjectId);
                    table.ForeignKey(
                        name: "FK_PortfolioProjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentProfiles",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    University = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Major = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    GPA = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true),
                    GithubUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProfiles", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_StudentProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCareerPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CareerPathId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCareerPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCareerPaths_CareerPaths_CareerPathId",
                        column: x => x.CareerPathId,
                        principalTable: "CareerPaths",
                        principalColumn: "CareerPathId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserCareerPaths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    UserSkillId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => x.UserSkillId);
                    table.ForeignKey(
                        name: "FK_UserSkills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningResources",
                columns: table => new
                {
                    ResourceId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SkillNodeId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningResources", x => x.ResourceId);
                    table.ForeignKey(
                        name: "FK_LearningResources_SkillNodes_SkillNodeId",
                        column: x => x.SkillNodeId,
                        principalTable: "SkillNodes",
                        principalColumn: "SkillNodeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoadmapProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillNodeId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadmapProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_RoadmapProgresses_SkillNodes_SkillNodeId",
                        column: x => x.SkillNodeId,
                        principalTable: "SkillNodes",
                        principalColumn: "SkillNodeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoadmapProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_ChatMessages_ChatSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "ChatSessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CareerPaths",
                columns: new[] { "CareerPathId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Builds server-side applications, APIs, databases, and integrations.", "Backend Developer" },
                    { 2, "Builds user interfaces and client-side web experiences.", "Frontend Developer" },
                    { 3, "Builds both frontend and backend parts of web applications.", "Full Stack Developer" },
                    { 4, "Builds mobile applications for Android, iOS, or cross-platform environments.", "Mobile Developer" },
                    { 5, "Automates deployment, infrastructure, monitoring, and delivery workflows.", "DevOps Engineer" },
                    { 6, "Builds data pipelines, storage systems, and analytical data platforms.", "Data Engineer" },
                    { 7, "Builds applications and systems powered by machine learning and AI models.", "AI Engineer" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@career.com", "System Administrator", "AQAAAAIAAYagAAAAEPJIP02MeRxP8l+CpoOY6pVMkmQIN7ld9iJtMl9x1GRoDJ6zGwdNwdGpMkAOmG/wGw==", "Admin" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "student@career.com", "Demo Student", "AQAAAAIAAYagAAAAEAfVa3S4ro2yj/3hgkVlmHMjtgqGRU8jwSU1Uegq6m7sROZtvVMHizvNjlwL1Hi5ew==", "Student" }
                });

            migrationBuilder.InsertData(
                table: "ChatSessions",
                columns: new[] { "SessionId", "CreatedAt", "UserId" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 4, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.InsertData(
                table: "PortfolioProjects",
                columns: new[] { "ProjectId", "Description", "GithubUrl", "ImportedAt", "RepositoryName", "TechStack", "UserId" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), "A career orientation and learning roadmap platform built with ASP.NET Core and React.", "https://github.com/demo/CareerPlatform", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), "CareerPlatform", "ASP.NET Core, EF Core, SQL Server, React", new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "A practice backend API project with authentication, CRUD endpoints, and SQL Server persistence.", "https://github.com/demo/BackendApiPractice", new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), "BackendApiPractice", "C#, ASP.NET Core, SQL Server", new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "SkillNodes",
                columns: new[] { "SkillNodeId", "CareerPathId", "Description", "Difficulty", "DisplayOrder", "EstimatedHours", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Learn C# syntax, types, collections, and language fundamentals.", 1, 1, 20, "C#" },
                    { 2, 1, "Understand classes, interfaces, inheritance, encapsulation, and polymorphism.", 1, 2, 16, "Object Oriented Programming" },
                    { 3, 1, "Build backend applications and APIs with ASP.NET Core.", 2, 3, 24, "ASP.NET Core" },
                    { 4, 1, "Design relational schemas and query data with SQL Server.", 1, 4, 18, "SQL Server" },
                    { 5, 1, "Design RESTful endpoints, status codes, validation, and error responses.", 2, 5, 16, "REST API" },
                    { 6, 1, "Implement token-based authentication and role-based authorization.", 2, 6, 16, "JWT Authentication" },
                    { 7, 1, "Learn container fundamentals for packaging and running backend applications.", 1, 7, 12, "Docker Basics" },
                    { 8, 2, "Structure web pages with semantic HTML.", 1, 1, 10, "HTML" },
                    { 9, 2, "Style responsive layouts with CSS fundamentals.", 1, 2, 14, "CSS" },
                    { 10, 2, "Learn JavaScript syntax, DOM interaction, and asynchronous programming.", 1, 3, 22, "JavaScript" },
                    { 11, 2, "Add static typing and safer structure to JavaScript applications.", 2, 4, 16, "TypeScript" },
                    { 12, 2, "Build component-based user interfaces with React.", 2, 5, 24, "React" },
                    { 13, 2, "Implement client-side routing and navigation in React apps.", 2, 6, 10, "React Router" },
                    { 14, 2, "Connect frontend applications to backend APIs using HTTP clients.", 2, 7, 14, "API Integration" }
                });

            migrationBuilder.InsertData(
                table: "StudentProfiles",
                columns: new[] { "ProfileId", "GPA", "GithubUrl", "Major", "University", "UserId" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), 3.5m, "https://github.com/demo", "Software Engineering", "FPT University", new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.InsertData(
                table: "UserCareerPaths",
                columns: new[] { "Id", "CareerPathId", "UserId" },
                values: new object[] { 1, 1, new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.InsertData(
                table: "UserSkills",
                columns: new[] { "UserSkillId", "SkillName", "UserId" },
                values: new object[,]
                {
                    { 1, "C#", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 2, "ASP.NET Core", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 3, "SQL Server", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4, "REST API", new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.InsertData(
                table: "ChatMessages",
                columns: new[] { "MessageId", "Content", "CreatedAt", "Role", "SessionId" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), "How should I continue learning backend development?", new DateTime(2026, 1, 4, 9, 1, 0, 0, DateTimeKind.Utc), "User", new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Focus next on Object Oriented Programming, JWT Authentication, and Docker Basics to round out your backend roadmap.", new DateTime(2026, 1, 4, 9, 2, 0, 0, DateTimeKind.Utc), "Assistant", new Guid("44444444-4444-4444-4444-444444444444") }
                });

            migrationBuilder.InsertData(
                table: "LearningResources",
                columns: new[] { "ResourceId", "SkillNodeId", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, "Microsoft Learn C#", "https://learn.microsoft.com/dotnet/csharp/" },
                    { 2, 1, "FreeCodeCamp C# Tutorial", "https://www.freecodecamp.org/news/tag/c-sharp/" },
                    { 3, 2, "Microsoft OOP Concepts", "https://learn.microsoft.com/dotnet/csharp/fundamentals/tutorials/oop" },
                    { 4, 2, "Object Oriented Programming in C#", "https://learn.microsoft.com/dotnet/csharp/fundamentals/object-oriented/" },
                    { 5, 3, "Microsoft ASP.NET Core Documentation", "https://learn.microsoft.com/aspnet/core/" },
                    { 6, 3, "ASP.NET Core Web API Tutorial", "https://learn.microsoft.com/aspnet/core/tutorials/first-web-api" },
                    { 7, 4, "SQL Server Documentation", "https://learn.microsoft.com/sql/sql-server/" },
                    { 8, 4, "SQL Server Tutorial", "https://learn.microsoft.com/sql/t-sql/tutorial-writing-transact-sql-statements" },
                    { 9, 5, "REST API Design Guidance", "https://learn.microsoft.com/azure/architecture/best-practices/api-design" },
                    { 10, 5, "HTTP Methods and Status Codes", "https://developer.mozilla.org/docs/Web/HTTP" },
                    { 11, 6, "JWT Authentication in ASP.NET Core", "https://learn.microsoft.com/aspnet/core/security/authentication/" },
                    { 12, 6, "JSON Web Tokens Introduction", "https://jwt.io/introduction" },
                    { 13, 7, "Docker Get Started", "https://docs.docker.com/get-started/" },
                    { 14, 7, "Docker for .NET Developers", "https://learn.microsoft.com/dotnet/architecture/microservices/container-docker-introduction/" },
                    { 15, 8, "MDN HTML Guide", "https://developer.mozilla.org/docs/Web/HTML" },
                    { 16, 8, "FreeCodeCamp HTML Tutorial", "https://www.freecodecamp.org/news/tag/html/" },
                    { 17, 9, "MDN CSS Guide", "https://developer.mozilla.org/docs/Web/CSS" },
                    { 18, 9, "Web.dev Learn CSS", "https://web.dev/learn/css" },
                    { 19, 10, "MDN JavaScript Guide", "https://developer.mozilla.org/docs/Web/JavaScript/Guide" },
                    { 20, 10, "JavaScript.info", "https://javascript.info/" },
                    { 21, 11, "TypeScript Handbook", "https://www.typescriptlang.org/docs/handbook/intro.html" },
                    { 22, 11, "TypeScript for JavaScript Programmers", "https://www.typescriptlang.org/docs/handbook/typescript-in-5-minutes.html" },
                    { 23, 12, "React Official Documentation", "https://react.dev/" },
                    { 24, 12, "React Crash Course", "https://www.freecodecamp.org/news/react-crash-course/" },
                    { 25, 13, "React Router Documentation", "https://reactrouter.com/" },
                    { 26, 13, "React Router Tutorial", "https://reactrouter.com/en/main/start/tutorial" },
                    { 27, 14, "Axios Documentation", "https://axios-http.com/docs/intro" },
                    { 28, 14, "Fetch API Guide", "https://developer.mozilla.org/docs/Web/API/Fetch_API/Using_Fetch" }
                });

            migrationBuilder.InsertData(
                table: "RoadmapProgresses",
                columns: new[] { "ProgressId", "CompletedAt", "IsCompleted", "SkillNodeId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), true, 1, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 2, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 3, null, false, 4, new Guid("22222222-2222-2222-2222-222222222222") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareerPaths_Name",
                table: "CareerPaths",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SessionId",
                table: "ChatMessages",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SessionId_CreatedAt",
                table: "ChatMessages",
                columns: new[] { "SessionId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_UserId",
                table: "ChatSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningResources_SkillNodeId",
                table: "LearningResources",
                column: "SkillNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioProjects_UserId",
                table: "PortfolioProjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioProjects_UserId_GithubUrl",
                table: "PortfolioProjects",
                columns: new[] { "UserId", "GithubUrl" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapProgresses_SkillNodeId",
                table: "RoadmapProgresses",
                column: "SkillNodeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapProgresses_UserId",
                table: "RoadmapProgresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadmapProgresses_UserId_SkillNodeId",
                table: "RoadmapProgresses",
                columns: new[] { "UserId", "SkillNodeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillNodes_CareerPathId",
                table: "SkillNodes",
                column: "CareerPathId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillNodes_CareerPathId_DisplayOrder",
                table: "SkillNodes",
                columns: new[] { "CareerPathId", "DisplayOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfiles_UserId",
                table: "StudentProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCareerPaths_CareerPathId",
                table: "UserCareerPaths",
                column: "CareerPathId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCareerPaths_UserId",
                table: "UserCareerPaths",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_UserId",
                table: "UserSkills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_UserId_SkillName",
                table: "UserSkills",
                columns: new[] { "UserId", "SkillName" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "LearningResources");

            migrationBuilder.DropTable(
                name: "PortfolioProjects");

            migrationBuilder.DropTable(
                name: "RoadmapProgresses");

            migrationBuilder.DropTable(
                name: "StudentProfiles");

            migrationBuilder.DropTable(
                name: "UserCareerPaths");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "ChatSessions");

            migrationBuilder.DropTable(
                name: "SkillNodes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CareerPaths");
        }
    }
}
