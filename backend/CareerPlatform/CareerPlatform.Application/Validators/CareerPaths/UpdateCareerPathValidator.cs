using CareerPlatform.Application.DTOs.CareerPaths;
using FluentValidation;

namespace CareerPlatform.Application.Validators.CareerPaths;

public class UpdateCareerPathValidator : AbstractValidator<UpdateCareerPathDto>
{
    public UpdateCareerPathValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(request => request.Description)
            .MaximumLength(500);
    }
}
