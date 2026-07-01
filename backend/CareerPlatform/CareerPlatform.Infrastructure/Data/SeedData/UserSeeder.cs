using CareerPlatform.Domain.Enums;

namespace CareerPlatform.Infrastructure.Data.SeedData;

public static class UserSeeder
{
    public static readonly Guid AdminUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid StudentUserId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid StudentProfileId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static object[] Users =>
    [
        new
        {
            UserId = AdminUserId,
            FullName = "System Administrator",
            Email = "admin@career.com",
            PasswordHash = "AQAAAAIAAYagAAAAEPJIP02MeRxP8l+CpoOY6pVMkmQIN7ld9iJtMl9x1GRoDJ6zGwdNwdGpMkAOmG/wGw==",
            Role = UserRole.Admin,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new
        {
            UserId = StudentUserId,
            FullName = "Demo Student",
            Email = "student@career.com",
            PasswordHash = "AQAAAAIAAYagAAAAEAfVa3S4ro2yj/3hgkVlmHMjtgqGRU8jwSU1Uegq6m7sROZtvVMHizvNjlwL1Hi5ew==",
            Role = UserRole.Student,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
    ];

    public static object[] StudentProfiles =>
    [
        new
        {
            ProfileId = StudentProfileId,
            UserId = StudentUserId,
            University = "FPT University",
            Major = "Software Engineering",
            GPA = 3.5m,
            GithubUrl = "https://github.com/demo"
        }
    ];
}
