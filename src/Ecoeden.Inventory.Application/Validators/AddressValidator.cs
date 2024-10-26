using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Inventory.Application.Validators;
public class AddressValidator : AbstractValidator<AddressDto>
{
    public AddressValidator()
    {
        RuleFor(a => a.StreetName)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.AddressStreetNameError.Code)
            .WithMessage(ApiError.AddressStreetNameError.Message);

        RuleFor(a => a.StreetNumber)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.AddressStreetNumberError.Code)
            .WithMessage(ApiError.AddressStreetNumberError.Message);

        RuleFor(a => a.City)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.AddressCityError.Code)
            .WithMessage(ApiError.AddressCityError.Message);

        RuleFor(a => a.District)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.AddressDistrictError.Code)
            .WithMessage(ApiError.AddressDistrictError.Message);

        RuleFor(a => a.State)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.AddressStateError.Code)
            .WithMessage(ApiError.AddressStateError.Message);

        RuleFor(a => a.PostCode)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.AddressPostcodeError.Code)
            .WithMessage(ApiError.AddressPostcodeError.Message);
    }
}
