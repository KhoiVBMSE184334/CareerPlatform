namespace CareerPlatform.Application.DTOs.Chats;

public class ChatRequestDto
{
    public Guid? SessionId { get; set; }

    public string Message { get; set; } = string.Empty;
}
