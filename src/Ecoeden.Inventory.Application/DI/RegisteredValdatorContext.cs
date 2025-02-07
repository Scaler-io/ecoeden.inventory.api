using Ecoeden.Inventory.Application.Features.Stocks.Command.DeleteStock;
using Ecoeden.Inventory.Application.Validators;
using Ecoeden.Inventory.Domain.Models.Dtos;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Inventory.Application.DI;
public static class RegisteredValdatorContext
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<SupplierDto>, SupplierValidator>();
        services.AddScoped<IValidator<CustomerDto>, CustomerValidator>();
        services.AddScoped<IValidator<UnitDto>, UnitValidator>();
        services.AddScoped<IValidator<StockDto>, ProductStockValidator>();
        services.AddScoped<IValidator<DeleteStockCommand>, DeleteStockRequestValidator>();
        return services;
    }
}
