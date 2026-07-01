using CareerPlatform.Application.DTOs.CareerPaths;
using FluentValidation;

namespace CareerPlatform.Application.Validators.CareerPaths;

public class CreateCareerPathValidator : AbstractValidator<CreateCareerPathDto>
{
    public CreateCareerPathValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.Description)
            .MaximumLength(500);
    }
}
