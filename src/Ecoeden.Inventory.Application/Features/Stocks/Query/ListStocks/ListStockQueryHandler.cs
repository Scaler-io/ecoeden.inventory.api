using AutoMapper;
using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Entities.SQL;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Application.Features.Stocks.Query.ListStocks;
public class ListStockQueryHandler(ILogger logger, 
    IMapper mapper, 
    IUnitOfWorkFactory unitOfWorkFactory,
    IOptions<AppConfigOption> option,
    ICacheServiceBuildFactory cahceFactory) 
    : IQueryHandler<ListStockQuery, Result<IReadOnlyList<StockDto>>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _option = option.Value;
    private readonly ICacheService _cacheService = cahceFactory.CreateService(CacheServiceType.Distributed);
    private readonly IBaseRepository<ProductStock> _stockRepository = unitOfWorkFactory.CreateUnitOfWork("ecoedenStock") .Repository<ProductStock>();

    public async Task<Result<IReadOnlyList<StockDto>>> Handle(ListStockQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - list all present stocks");

        var cachedResult = await _cacheService.GetAsync<IReadOnlyList<StockDto>>(_option.StockStorageCacheKey, cancellationToken);

        if (cachedResult is not null)
        {
            _logger.Here().Information("ListStock - cahce hit");
            return Result<IReadOnlyList<StockDto>>.Success(cachedResult);
        }

        _logger.Here().Information("ListStock - cache miss");

        var stockData = await _stockRepository.ListAllAsync();

        if(stockData is null || stockData.Count == 0 )
        {
            _logger.Here().Error("No stock is present");
            return Result<IReadOnlyList<StockDto>>.Faliure(ErrorCodes.NotFound);
        }

        var mappedResult = _mapper.Map<IReadOnlyList<StockDto>>(stockData);
        await _cacheService.SetAsync(_option.StockStorageCacheKey, mappedResult, cancellation: cancellationToken);

        _logger.Here().Information("Total {count} stocks were found", stockData.Count);
        _logger.Here().MethodExited();

        return Result<IReadOnlyList<StockDto>>.Success(mappedResult);
    }
}
