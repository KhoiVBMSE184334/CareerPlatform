namespace CareerPlatform.Domain.Entities;

public class ChatSession
{
    public Guid SessionId { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;

    public ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
}
