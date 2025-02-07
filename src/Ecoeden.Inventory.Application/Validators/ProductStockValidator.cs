using Ecoeden.Inventory.Application.Features.Stocks.Command.DeleteStock;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;

namespace Ecoeden.Inventory.Application.Validators;
public class ProductStockValidator: AbstractValidator<StockDto>
{
    public ProductStockValidator()
    {
        RuleFor(s => s.SupplierId)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.StockSupplierError.Code)
            .WithMessage(ApiError.StockSupplierError.Message);

        RuleFor(s => s.ProductId)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.StockProductError.Code)
            .WithMessage(ApiError.StockProductError.Message);
    }
}


public class DeleteStockRequestValidator: AbstractValidator<DeleteStockCommand>
{
    public DeleteStockRequestValidator()
    {
        RuleFor(s => s.ProductId)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.StockProductError.Code)
            .WithMessage(ApiError.StockProductError.Message);

        RuleFor(s => s.SupplierId)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithErrorCode(ApiError.StockSupplierError.Code)
            .WithMessage(ApiError.StockSupplierError.Message);
    }
}