using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Inventory.Application.Validators;
public class SupplierValidator : AbstractValidator<SupplierDto>
{
    public SupplierValidator()
    {

        RuleFor(s => s.Name)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.SupplierNameError.Code)
            .WithMessage(ApiError.SupplierNameError.Message);

        RuleFor(s => s.ContactDetails)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.SupplierContactError.Code)
            .WithMessage(ApiError.SupplierContactError.Message)
            .SetValidator(new ContactDetailsValidator());
    }
}
