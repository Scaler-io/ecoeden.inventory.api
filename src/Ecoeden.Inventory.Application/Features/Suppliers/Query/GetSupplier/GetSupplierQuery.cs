using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Query.GetSupplier;
public class GetSupplierQuery(string id, RequestInformation requestInformation) : IQuery<Result<SupplierDto>>
{
    public string Id { get; set; } = id;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
