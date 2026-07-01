using CareerPlatform.Application.Interfaces.Repositories;
using CareerPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPlatform.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ChatSession>> GetSessionsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.ChatSessions
            .Include(session => session.ChatMessages)
            .Where(session => session.UserId == userId)
            .OrderByDescending(session => session.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<ChatSession?> GetSessionByIdAsync(Guid sessionId, Guid userId, CancellationToken cancellationToken = default)
    {
        return _context.ChatSessions
            .Include(session => session.ChatMessages)
            .FirstOrDefaultAsync(
                session => session.SessionId == sessionId && session.UserId == userId,
                cancellationToken);
    }

    public async Task AddSessionAsync(ChatSession chatSession, CancellationToken cancellationToken = default)
    {
        await _context.ChatSessions.AddAsync(chatSession, cancellationToken);
    }

    public async Task AddMessageAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default)
    {
        await _context.ChatMessages.AddAsync(chatMessage, cancellationToken);
    }
}
