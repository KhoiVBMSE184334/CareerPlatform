using CareerPlatform.Application.DTOs.Chats;
using FluentValidation;

namespace CareerPlatform.Application.Validators.Chats;

public class ChatRequestValidator : AbstractValidator<ChatRequestDto>
{
    public ChatRequestValidator()
    {
        RuleFor(request => request.Message)
            .NotEmpty()
            .MaximumLength(4000);
    }
}
