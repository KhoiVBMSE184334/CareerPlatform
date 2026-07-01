using CareerPlatform.Application.DTOs.Chats;

namespace CareerPlatform.Application.Interfaces.Services;

public interface IAIChatService
{
    Task<ChatResponseDto> SendMessageAsync(Guid userId, ChatRequestDto request, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ChatSessionDto>> GetSessionsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ChatSessionDto?> GetSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default);
}
