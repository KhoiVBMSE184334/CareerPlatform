namespace CareerPlatform.Application.Interfaces.Services;

public interface IGeminiService
{
    Task<string> GenerateMentorResponseAsync(string prompt, CancellationToken cancellationToken = default);

    Task<string> SummarizeReadmeAsync(
        string repositoryName,
        string readmeContent,
        CancellationToken cancellationToken = default);
}
