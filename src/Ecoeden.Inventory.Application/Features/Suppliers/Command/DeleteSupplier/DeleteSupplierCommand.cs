using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Command.DeleteSupplier;
public class DeleteSupplierCommand(string id, RequestInformation requestInformation) : ICommand<Result<bool>>
{
    public string Id { get; set; } = id;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
