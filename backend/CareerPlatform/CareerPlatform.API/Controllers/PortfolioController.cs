using System.Security.Claims;
using CareerPlatform.Application.DTOs.Portfolios;
using CareerPlatform.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/portfolio")]
public class PortfolioController : ControllerBase
{
    private readonly IPortfolioService _portfolioService;

    public PortfolioController(IPortfolioService portfolioService)
    {
        _portfolioService = portfolioService;
    }

    [HttpPost("import-github")]
    public async Task<ActionResult<IReadOnlyList<PortfolioProjectDto>>> ImportGitHub(
        ImportGitHubRequestDto request,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        try
        {
            var projects = await _portfolioService.ImportFromGitHubAsync(userId.Value, request, cancellationToken);
            return Ok(projects);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PortfolioProjectDto>>> GetPortfolio(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        var projects = await _portfolioService.GetMyPortfolioAsync(userId.Value, cancellationToken);
        return Ok(projects);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<ActionResult<IReadOnlyList<AdminPortfolioProjectDto>>> GetAdminPortfolio(CancellationToken cancellationToken)
    {
        var projects = await _portfolioService.GetAllForAdminAsync(cancellationToken);
        return Ok(projects);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin/{id:guid}")]
    public async Task<ActionResult<AdminPortfolioProjectDto>> GetAdminPortfolioProject(
        Guid id,
        CancellationToken cancellationToken)
    {
        var project = await _portfolioService.GetByIdForAdminAsync(id, cancellationToken);

        if (project is null)
        {
            return NotFound(new { message = "Portfolio project was not found." });
        }

        return Ok(project);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        try
        {
            await _portfolioService.DeleteAsync(userId.Value, id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    private Guid? GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }
}
