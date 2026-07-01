namespace CareerPlatform.Application.DTOs.Chats;

public class ChatSessionDto
{
    public Guid SessionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public IReadOnlyList<ChatMessageDto> Messages { get; set; } = [];
}
