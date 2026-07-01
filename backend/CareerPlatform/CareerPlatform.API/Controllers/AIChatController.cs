using System.Security.Claims;
using CareerPlatform.Application.DTOs.Chats;
using CareerPlatform.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerPlatform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/chat")]
public class AIChatController : ControllerBase
{
    private readonly IAIChatService _aiChatService;
    private readonly IValidator<ChatRequestDto> _chatRequestValidator;

    public AIChatController(
        IAIChatService aiChatService,
        IValidator<ChatRequestDto> chatRequestValidator)
    {
        _aiChatService = aiChatService;
        _chatRequestValidator = chatRequestValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ChatResponseDto>> SendMessage(
        ChatRequestDto request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _chatRequestValidator.ValidateAsync(request, cancellationToken);

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
            var response = await _aiChatService.SendMessageAsync(userId.Value, request, cancellationToken);
            return Ok(response);
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

    [HttpGet("sessions")]
    public async Task<ActionResult<IReadOnlyList<ChatSessionDto>>> GetSessions(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        var sessions = await _aiChatService.GetSessionsAsync(userId.Value, cancellationToken);
        return Ok(sessions);
    }

    [HttpGet("sessions/{sessionId:guid}")]
    public async Task<ActionResult<ChatSessionDto>> GetSession(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
        {
            return Unauthorized(new { message = "User identifier is missing from the token." });
        }

        var session = await _aiChatService.GetSessionAsync(userId.Value, sessionId, cancellationToken);

        if (session is null)
        {
            return NotFound(new { message = "Chat session was not found." });
        }

        return Ok(session);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }
}
