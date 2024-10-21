using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Inventory.Application.Validators;
public class SupplierValidator : AbstractValidator<SupplierDto>
{
    public SupplierValidator()
    {
        //RuleFor(s => s.Id).NotEmpty()
        //    .WithErrorCode(ApiError.SupplierNameError.Code)
        //    .WithMessage(ApiError.SupplierNameError.Message);


        //RuleFor(s => s.Phone)
        //    .NotEmpty()
        //    .WithErrorCode(ApiError.SupplierPhoneError.Code)
        //    .WithMessage(ApiError.SupplierPhoneError.Message);

        //RuleFor(s => s.Phone)
        //    .Must(phone => PhoneValidator.IsValid(phone))
        //    .WithErrorCode(ApiError.InvalidPhoneNumberError.Code)
        //    .WithMessage(ApiError.InvalidPhoneNumberError.Message)
        //    .When(s => s.Phone.Length > 0);
           
            
    }
}
