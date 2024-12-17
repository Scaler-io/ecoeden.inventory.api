using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Inventory.Application.Validators;
public class CustomerValidator : AbstractValidator<CustomerDto>
{
    public CustomerValidator()
    {
        RuleFor(s => s.Name)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.CustomerNameError.Code)
            .WithMessage(ApiError.CustomerNameError.Message);

        RuleFor(s => s.ContactDetails)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.CustomerContactError.Code)
            .WithMessage(ApiError.CustomerContactError.Message)
            .SetValidator(new ContactDetailsValidator());
    }
}
