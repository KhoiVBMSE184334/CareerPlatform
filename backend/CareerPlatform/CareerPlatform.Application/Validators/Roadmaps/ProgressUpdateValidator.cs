using CareerPlatform.Application.DTOs.Roadmaps;
using FluentValidation;

namespace CareerPlatform.Application.Validators.Roadmaps;

public class ProgressUpdateValidator : AbstractValidator<ProgressUpdateDto>
{
    public ProgressUpdateValidator()
    {
        RuleFor(request => request.SkillNodeId)
            .GreaterThan(0);
    }
}
