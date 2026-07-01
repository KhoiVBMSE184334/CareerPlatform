using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CareerPlatform.Infrastructure.Services;

public class GeminiService : IGeminiService
{
    private const string DefaultModel = "gemini-2.5-flash";
    private const string DefaultEndpointTemplate = "https://generativelanguage.googleapis.com/v1beta/models/{0}:generateContent?key={1}";
    private const string UnavailableResponse = "AI service is currently unavailable. Please check the API key or try again later.";

    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GeminiService> _logger;

    public GeminiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<GeminiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GenerateMentorResponseAsync(
        string prompt,
        CancellationToken cancellationToken = default)
    {
        if (IsMissingApiKey(_configuration["Gemini:ApiKey"]))
        {
            return BuildDemoMentorResponse();
        }

        return await SendGenerateContentAsync(prompt, cancellationToken);
    }

    public async Task<string> SummarizeReadmeAsync(
        string repositoryName,
        string readmeContent,
        CancellationToken cancellationToken = default)
    {
        if (IsMissingApiKey(_configuration["Gemini:ApiKey"]))
        {
            return $"Demo summary for {repositoryName}: This repository can be presented as a portfolio project. Add a concise README summary by configuring Gemini:ApiKey.";
        }

        var prompt = $"""
            Summarize this GitHub README for a student portfolio.
            Focus on project purpose, main features, and technologies.
            Do not inspect or infer from source code.

            Repository: {repositoryName}

            README:
            {readmeContent[..Math.Min(readmeContent.Length, 12000)]}
            """;

        return await SendGenerateContentAsync(prompt, cancellationToken);
    }

    private async Task<string> SendGenerateContentAsync(
        string prompt,
        CancellationToken cancellationToken)
    {
        var apiKey = _configuration["Gemini:ApiKey"];

        if (IsMissingApiKey(apiKey))
        {
            return BuildDemoMentorResponse();
        }

        var model = string.IsNullOrWhiteSpace(_configuration["Gemini:Model"])
            ? DefaultModel
            : _configuration["Gemini:Model"];

        var endpoint = string.Format(DefaultEndpointTemplate, Uri.EscapeDataString(model!), Uri.EscapeDataString(apiKey!));

        try
        {
            using var response = await _httpClient.PostAsJsonAsync(
                endpoint,
                new GeminiGenerateContentRequest
                {
                    Contents =
                    [
                        new GeminiContent
                        {
                            Parts =
                            [
                                new GeminiPart { Text = prompt }
                            ]
                        }
                    ]
                },
                cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Gemini request failed with status {StatusCode}: {ResponseBody}",
                    response.StatusCode,
                    responseBody);

                return UnavailableResponse;
            }

            var completion = JsonSerializer.Deserialize<GeminiGenerateContentResponse>(
                responseBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var content = completion?
                .Candidates
                .FirstOrDefault()?
                .Content?
                .Parts
                .FirstOrDefault()?
                .Text;

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogError("Gemini returned an empty response body: {ResponseBody}", responseBody);
                return UnavailableResponse;
            }

            return content.Trim();
        }
        catch (HttpRequestException exception)
        {
            _logger.LogError(exception, "Gemini HTTP request failed.");
            return UnavailableResponse;
        }
        catch (JsonException exception)
        {
            _logger.LogError(exception, "Gemini response could not be parsed.");
            return UnavailableResponse;
        }
    }

    private static bool IsMissingApiKey(string? apiKey)
    {
        return string.IsNullOrWhiteSpace(apiKey)
            || apiKey.Equals("YOUR_GEMINI_API_KEY", StringComparison.OrdinalIgnoreCase)
            || apiKey.Equals("YOUR_OPENAI_API_KEY", StringComparison.OrdinalIgnoreCase)
            || apiKey.Equals("YOUR_KEY_HERE", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildDemoMentorResponse()
    {
        return "AI Mentor is running in demo mode. Based on the student's career roadmap, focus on the next missing skills, build one practical portfolio project, and keep learning steps small and measurable.";
    }

    private sealed class GeminiGenerateContentRequest
    {
        [JsonPropertyName("contents")]
        public IReadOnlyList<GeminiContent> Contents { get; set; } = [];
    }

    private sealed class GeminiGenerateContentResponse
    {
        [JsonPropertyName("candidates")]
        public List<GeminiCandidate> Candidates { get; set; } = [];
    }

    private sealed class GeminiCandidate
    {
        [JsonPropertyName("content")]
        public GeminiContent? Content { get; set; }
    }

    private sealed class GeminiContent
    {
        [JsonPropertyName("parts")]
        public IReadOnlyList<GeminiPart> Parts { get; set; } = [];
    }

    private sealed class GeminiPart
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
