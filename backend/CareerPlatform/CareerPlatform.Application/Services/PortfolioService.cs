using CareerPlatform.Application.DTOs.Portfolios;
using CareerPlatform.Application.Interfaces;
using CareerPlatform.Application.Interfaces.Services;
using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGitHubService _gitHubService;
    private readonly IGeminiService _geminiService;

    public PortfolioService(
        IUnitOfWork unitOfWork,
        IGitHubService gitHubService,
        IGeminiService geminiService)
    {
        _unitOfWork = unitOfWork;
        _gitHubService = gitHubService;
        _geminiService = geminiService;
    }

    public async Task<IReadOnlyList<AdminPortfolioProjectDto>> GetAllForAdminAsync(CancellationToken cancellationToken = default)
    {
        var projects = await _unitOfWork.PortfolioProjects.GetAllAsync(cancellationToken);
        return projects.Select(MapToAdminDto).ToList();
    }

    public async Task<AdminPortfolioProjectDto?> GetByIdForAdminAsync(
        Guid projectId,
        CancellationToken cancellationToken = default)
    {
        var project = await _unitOfWork.PortfolioProjects.GetByIdAsync(projectId, cancellationToken);
        return project is null ? null : MapToAdminDto(project);
    }

    public async Task<IReadOnlyList<PortfolioProjectDto>> ImportFromGitHubAsync(
        Guid userId,
        ImportGitHubRequestDto request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.GithubUrlOrUsername))
        {
            throw new InvalidOperationException("GitHub profile URL or username is required.");
        }

        var repositories = await _gitHubService.GetPublicRepositoriesAsync(
            request.GithubUrlOrUsername,
            cancellationToken);

        var importedProjects = new List<PortfolioProject>();

        foreach (var repository in repositories)
        {
            var description = repository.Description;

            if (!string.IsNullOrWhiteSpace(repository.ReadmeContent))
            {
                description = await _geminiService.SummarizeReadmeAsync(
                    repository.Name,
                    repository.ReadmeContent,
                    cancellationToken);
            }

            var existingProject = await _unitOfWork.PortfolioProjects.GetByGithubUrlAsync(
                userId,
                repository.GithubUrl,
                cancellationToken);

            if (existingProject is null)
            {
                existingProject = new PortfolioProject
                {
                    ProjectId = Guid.NewGuid(),
                    UserId = userId,
                    GithubUrl = repository.GithubUrl,
                    ImportedAt = DateTime.UtcNow
                };

                await _unitOfWork.PortfolioProjects.AddAsync(existingProject, cancellationToken);
            }

            existingProject.RepositoryName = repository.Name;
            existingProject.Description = description;
            existingProject.TechStack = repository.Language;
            existingProject.ImportedAt = DateTime.UtcNow;

            _unitOfWork.PortfolioProjects.Update(existingProject);
            importedProjects.Add(existingProject);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return importedProjects.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<PortfolioProjectDto>> GetMyPortfolioAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var projects = await _unitOfWork.PortfolioProjects.GetByUserIdAsync(userId, cancellationToken);
        return projects.Select(MapToDto).ToList();
    }

    public async Task DeleteAsync(Guid userId, Guid projectId, CancellationToken cancellationToken = default)
    {
        var project = await _unitOfWork.PortfolioProjects.GetByIdAsync(projectId, userId, cancellationToken);

        if (project is null)
        {
            throw new KeyNotFoundException("Portfolio project was not found.");
        }

        _unitOfWork.PortfolioProjects.Delete(project);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static PortfolioProjectDto MapToDto(PortfolioProject project)
    {
        return new PortfolioProjectDto
        {
            ProjectId = project.ProjectId,
            RepositoryName = project.RepositoryName,
            Description = project.Description,
            TechStack = project.TechStack,
            GithubUrl = project.GithubUrl,
            ImportedAt = project.ImportedAt
        };
    }

    private static AdminPortfolioProjectDto MapToAdminDto(PortfolioProject project)
    {
        return new AdminPortfolioProjectDto
        {
            ProjectId = project.ProjectId,
            UserId = project.UserId,
            StudentName = project.User.FullName,
            RepositoryName = project.RepositoryName,
            Description = project.Description,
            TechStack = project.TechStack,
            GithubUrl = project.GithubUrl,
            ImportedAt = project.ImportedAt
        };
    }
}
