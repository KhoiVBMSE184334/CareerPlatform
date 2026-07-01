namespace CareerPlatform.Application.DTOs.Chats;

public class ChatMessageDto
{
    public Guid MessageId { get; set; }

    public string Role { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
