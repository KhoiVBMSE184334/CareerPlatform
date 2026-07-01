using CareerPlatform.Application.DTOs.SkillGaps;
using FluentValidation;

namespace CareerPlatform.Application.Validators.SkillGaps;

public class SkillGapRequestValidator : AbstractValidator<SkillGapRequestDto>
{
    public SkillGapRequestValidator()
    {
        RuleFor(request => request.CareerPathId)
            .GreaterThan(0)
            .When(request => request.CareerPathId.HasValue);

        RuleFor(request => request.Skills)
            .NotNull()
            .Must(skills => skills.Count > 0)
            .WithMessage("At least one skill is required.");

        RuleForEach(request => request.Skills)
            .NotEmpty()
            .MaximumLength(100);
    }
}
