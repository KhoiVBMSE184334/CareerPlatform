using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using CareerPlatform.Application.DTOs.Portfolios;
using CareerPlatform.Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace CareerPlatform.Infrastructure.Services;

public class GitHubService : IGitHubService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GitHubService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IReadOnlyList<GitHubRepoDto>> GetPublicRepositoriesAsync(
        string githubUrlOrUsername,
        CancellationToken cancellationToken = default)
    {
        var username = ExtractUsername(githubUrlOrUsername);

        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidOperationException("GitHub profile URL or username is invalid.");
        }

        ConfigureHeaders();

        var repositories = await _httpClient.GetFromJsonAsync<List<GitHubRepositoryResponse>>(
            $"https://api.github.com/users/{username}/repos?type=owner&sort=updated&per_page=100",
            cancellationToken) ?? [];

        var result = new List<GitHubRepoDto>();

        foreach (var repository in repositories.Where(repository => !repository.Fork))
        {
            var readmeContent = await TryGetReadmeContentAsync(username, repository.Name, cancellationToken);

            result.Add(new GitHubRepoDto
            {
                Name = repository.Name,
                Description = repository.Description,
                Language = repository.Language,
                GithubUrl = repository.HtmlUrl,
                ReadmeContent = readmeContent
            });
        }

        return result;
    }

    private void ConfigureHeaders()
    {
        _httpClient.DefaultRequestHeaders.UserAgent.Clear();
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CareerPlatform", "1.0"));
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

        var token = _configuration["GitHub:Token"];

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task<string?> TryGetReadmeContentAsync(
        string username,
        string repositoryName,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://api.github.com/repos/{username}/{repositoryName}/readme");

        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("CareerPlatform", "1.0"));
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.raw"));

        var token = _configuration["GitHub:Token"];

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        return string.IsNullOrWhiteSpace(content) ? null : content;
    }

    private static string ExtractUsername(string githubUrlOrUsername)
    {
        var value = githubUrlOrUsername.Trim().TrimEnd('/');

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            return value.TrimStart('@');
        }

        if (!uri.Host.Contains("github.com", StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        return uri.AbsolutePath
            .Trim('/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault() ?? string.Empty;
    }

    private sealed class GitHubRepositoryResponse
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;

        [JsonPropertyName("fork")]
        public bool Fork { get; set; }
    }
}
