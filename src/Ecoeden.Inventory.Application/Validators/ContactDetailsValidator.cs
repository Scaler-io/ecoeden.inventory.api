using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Inventory.Application.Validators;
public class ContactDetailsValidator : AbstractValidator<ContactDetailsDto>
{
    public ContactDetailsValidator()
    {
        RuleFor(c => c.Phone)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.ContactDetailsPhoneError.Code)
            .WithMessage(ApiError.ContactDetailsPhoneError.Message)
            .Must(c => PhoneValidator.IsValid(c))
            .WithErrorCode(ApiError.ContactDetailsPhoneInvalidError.Code)
            .WithMessage(ApiError.ContactDetailsPhoneInvalidError.Message);

        RuleFor(c => c.Email)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.ContactDetailsEmailError.Code)
            .WithMessage(ApiError.ContactDetailsEmailError.Message);

        RuleFor(c => c.Address)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.ContactDetialsAddressError.Code)
            .WithMessage(ApiError.ContactDetialsAddressError.Message)
            .SetValidator(new AddressValidator());

    }
}
