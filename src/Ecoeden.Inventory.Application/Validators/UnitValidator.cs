using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Inventory.Application.Validators;
public class UnitValidator : AbstractValidator<UnitDto>
{
    public UnitValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .WithErrorCode(ApiError.UnitNameError.Code)
            .WithMessage(ApiError.UnitNameError.Message);
    }
}
