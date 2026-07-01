namespace CareerPlatform.Infrastructure.Data.SeedData;

public static class CareerPathSeeder
{
    public static object[] CareerPaths =>
    [
        new
        {
            CareerPathId = 1,
            Name = "Backend Developer",
            Description = "Builds server-side applications, APIs, databases, and integrations."
        },
        new
        {
            CareerPathId = 2,
            Name = "Frontend Developer",
            Description = "Builds user interfaces and client-side web experiences."
        },
        new
        {
            CareerPathId = 3,
            Name = "Full Stack Developer",
            Description = "Builds both frontend and backend parts of web applications."
        },
        new
        {
            CareerPathId = 4,
            Name = "Mobile Developer",
            Description = "Builds mobile applications for Android, iOS, or cross-platform environments."
        },
        new
        {
            CareerPathId = 5,
            Name = "DevOps Engineer",
            Description = "Automates deployment, infrastructure, monitoring, and delivery workflows."
        },
        new
        {
            CareerPathId = 6,
            Name = "Data Engineer",
            Description = "Builds data pipelines, storage systems, and analytical data platforms."
        },
        new
        {
            CareerPathId = 7,
            Name = "AI Engineer",
            Description = "Builds applications and systems powered by machine learning and AI models."
        }
    ];
}
