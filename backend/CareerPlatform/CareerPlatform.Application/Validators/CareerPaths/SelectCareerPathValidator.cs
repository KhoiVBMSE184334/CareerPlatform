using CareerPlatform.Application.DTOs.CareerPaths;
using FluentValidation;

namespace CareerPlatform.Application.Validators.CareerPaths;

public class SelectCareerPathValidator : AbstractValidator<SelectCareerPathDto>
{
    public SelectCareerPathValidator()
    {
        RuleFor(request => request.CareerPathId)
            .GreaterThan(0);
    }
}
