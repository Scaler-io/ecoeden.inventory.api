﻿using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Stocks.Query.ListStocks;
public class ListStockQuery : IQuery<Result<IReadOnlyList<StockDto>>>
{
}
