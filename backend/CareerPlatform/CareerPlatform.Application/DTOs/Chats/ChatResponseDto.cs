namespace CareerPlatform.Application.DTOs.Chats;

public class ChatResponseDto
{
    public Guid SessionId { get; set; }

    public string Response { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
