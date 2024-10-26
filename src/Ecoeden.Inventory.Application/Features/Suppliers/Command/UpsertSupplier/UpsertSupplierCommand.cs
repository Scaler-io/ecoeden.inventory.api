using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Attributes;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Command.UpsertSupplier;
public class UpsertSupplierCommand(SupplierDto supplier, RequestInformation requestInformation) : ICommand<Result<SupplierDto>>
{
    [ValidateNested]
    public SupplierDto Supplier { get; set; } = supplier;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
