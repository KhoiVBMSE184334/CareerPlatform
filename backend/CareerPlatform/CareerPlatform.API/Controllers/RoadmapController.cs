using System.Security.Claims;
using CareerPlatform.Application.DTOs.Roadmaps;
using CareerPlatform.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/roadmap")]
public class RoadmapController : ControllerBase
{
    private readonly IRoadmapService _roadmapService;
    private readonly IValidator<ProgressUpdateDto> _progressUpdateValidator;

    public RoadmapController(
        IRoadmapService roadmapService,
        IValidator<ProgressUpdateDto> progressUpdateValidator)
    {
        _roadmapService = roadmapService;
        _progressUpdateValidator = progressUpdateValidator;
    }

    [HttpGet]
    public async Task<ActionResult<RoadmapDto>> GetSelectedRoadmap(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        try
        {
            var roadmap = await _roadmapService.GetSelectedRoadmapAsync(userId.Value, cancellationToken);
            return Ok(roadmap);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpGet("career-path/{careerPathId:int}")]
    public async Task<ActionResult<RoadmapDto>> GetByCareerPath(
        int careerPathId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        try
        {
            var roadmap = await _roadmapService.GetRoadmapByCareerPathAsync(userId.Value, careerPathId, cancellationToken);
            return Ok(roadmap);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPut("progress")]
    public async Task<ActionResult<RoadmapDto>> UpdateProgress(
        ProgressUpdateDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _progressUpdateValidator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { errors = validationResult.ToDictionary() });
        }

        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        try
        {
            var roadmap = await _roadmapService.UpdateProgressAsync(userId.Value, request, cancellationToken);
            return Ok(roadmap);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
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
