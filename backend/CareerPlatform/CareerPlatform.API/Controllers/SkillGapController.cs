using System.Security.Claims;
using CareerPlatform.Application.DTOs.SkillGaps;
using CareerPlatform.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/skillgap")]
public class SkillGapController : ControllerBase
{
    private readonly ISkillGapService _skillGapService;
    private readonly IValidator<SkillGapRequestDto> _skillGapRequestValidator;

    public SkillGapController(
        ISkillGapService skillGapService,
        IValidator<SkillGapRequestDto> skillGapRequestValidator)
    {
        _skillGapService = skillGapService;
        _skillGapRequestValidator = skillGapRequestValidator;
    }

    [HttpPost("analyze")]
    public async Task<ActionResult<SkillGapResultDto>> Analyze(
        SkillGapRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _skillGapRequestValidator.ValidateAsync(request, cancellationToken);

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
            var result = await _skillGapService.AnalyzeAsync(userId.Value, request, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpGet("my-result")]
    public async Task<ActionResult<SkillGapResultDto>> GetMyResult(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        try
        {
            var result = await _skillGapService.GetMyResultAsync(userId.Value, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    private Guid? GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }
}
