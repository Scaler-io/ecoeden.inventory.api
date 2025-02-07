using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;

namespace Ecoeden.Inventory.Application.Features.Stocks.Command.DeleteStock;
public class DeleteStockCommand(string productId, string supplierId, RequestInformation requestInformation) : ICommand<Result<bool>>
{
    public string ProductId { get; set; } = productId;
    public string SupplierId { get; set; } = supplierId;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}