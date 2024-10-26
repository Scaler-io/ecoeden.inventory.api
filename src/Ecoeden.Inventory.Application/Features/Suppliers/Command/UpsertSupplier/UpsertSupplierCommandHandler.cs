using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Features.Suppliers.Command.UpsertSupplier;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Command.CreateSupplier;
public class UpsertSupplierCommandHandler : ICommandHandler<UpsertSupplierCommand, Result<SupplierDto>>
{
    public async Task<Result<SupplierDto>> Handle(UpsertSupplierCommand request, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return Result<SupplierDto>.Success(request.Supplier);
    }
}
