using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Query.ListSuppliers;
public class ListSuppliersQuery(RequestInformation requestInformation) : IQuery<Result<IReadOnlyList<SupplierDto>>>
{
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
