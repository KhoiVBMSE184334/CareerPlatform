using CareerPlatform.Domain.Entities;

namespace CareerPlatform.Application.Interfaces.Repositories;

public interface IChatRepository
{
    Task<IReadOnlyList<ChatSession>> GetSessionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ChatSession?> GetSessionByIdAsync(Guid sessionId, Guid userId, CancellationToken cancellationToken = default);

    Task AddSessionAsync(ChatSession chatSession, CancellationToken cancellationToken = default);

    Task AddMessageAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default);
}
