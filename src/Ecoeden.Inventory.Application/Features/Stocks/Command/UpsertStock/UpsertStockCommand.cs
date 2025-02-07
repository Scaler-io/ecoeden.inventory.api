using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Attributes;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Stocks.Command.UpsertStock;
public class UpsertStockCommand(StockDto stock, RequestInformation requestInformation) 
    : ICommand<Result<StockDto>>
{
    [ValidateNested]
    public StockDto Stock { get; set; } = stock;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
