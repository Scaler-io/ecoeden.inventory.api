using AutoMapper;
using Contracts.Events;
using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Application.Contracts.EventBus;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Entities.SQL;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Application.Features.Stocks.Command.UpsertStock;
public class UpsertStockCommandHandler(ILogger logger, 
    IUnitOfWorkFactory unitOfWorkFactory, 
    IOptions<AppConfigOption> options, 
    ICacheServiceBuildFactory cacheServiceBuildFactory, 
    IPublishServiceFactory publishServiceFactory,
    IMapper mapper) : ICommandHandler<UpsertStockCommand, Result<StockDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _appConfigOptions = options.Value;
    private readonly IUnitOfWork _unitOfWork = unitOfWorkFactory.CreateUnitOfWork("ecoedenStock");
    private readonly ICacheService _cacheService = cacheServiceBuildFactory.CreateService(CacheServiceType.Distributed);
    private readonly IBaseRepository<ProductStock> _stockRepository = unitOfWorkFactory.CreateUnitOfWork("ecoedenStock").Repository<ProductStock>();
    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;

    public async Task<Result<StockDto>> Handle(UpsertStockCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
             .Information("Request - create or update stock for {@stock}", request.Stock);

        var stockExists = await _stockRepository.GetEntityByPredicate(stock => stock.ProductId == request.Stock.ProductId && stock.SupplierId == request.Stock.SupplierId);

        Result<StockDto> stockUpsertResult;

        if (stockExists == null)
        {
            var stockToCreate = (ProductStock)_mapper.Map(request.Stock, typeof(StockDto), typeof(ProductStock));
            stockUpsertResult = await CreateStock(stockToCreate, request.RequestInformation);
        }
        else
        {
            var stockToUpdate = (ProductStock)_mapper.Map(request.Stock, stockExists, typeof(StockDto), typeof(ProductStock));
            stockUpsertResult = await UpdateStock(stockToUpdate, request.RequestInformation);
        }

        await _cacheService.RemoveAsync(_appConfigOptions.StockStorageCacheKey, cancellationToken);

        _logger.Here().MethodExited();
        return stockUpsertResult;
    }

    private async Task<Result<StockDto>> CreateStock(ProductStock stockEntity, RequestInformation requestInformation)
    {
        try
        {
            stockEntity.UpdateCreationData(requestInformation.CurrentUser.Id, requestInformation.CorrelationId);
            _stockRepository.Add(stockEntity);
            var successCode = await _unitOfWork.Complete();

            if(successCode > 0)
            {
                var stockCreatedPublishService = _publishServiceFactory.CreatePublishService<ProductStock, ProductStockCreated>();
                await stockCreatedPublishService.PublishAsync(stockEntity, requestInformation.CorrelationId, GetApplicationMetaData());
            }

            return Result<StockDto>.Success(_mapper.Map<StockDto>(stockEntity));
        }
        catch (Exception ex)
        {
            _logger.Here().Error("Failed to create stock {ex}", ex);
            return Result<StockDto>.Faliure(ErrorCodes.OperationFailed);
        } 
    }

    private async Task<Result<StockDto>> UpdateStock(ProductStock stockEntity, RequestInformation requestInformation)
    {
        try
        {
            stockEntity.UpdateUpdationData(requestInformation.CurrentUser.Id, requestInformation.CorrelationId);
            _stockRepository.Update(stockEntity);
            var successCode = await _unitOfWork.Complete();

            if(successCode > 0)
            {
                var stockUpdatedPublishService = _publishServiceFactory.CreatePublishService<ProductStock, ProductStockUpdated>();
                await stockUpdatedPublishService.PublishAsync(stockEntity, requestInformation.CorrelationId, GetApplicationMetaData());
            }

            return Result<StockDto>.Success(_mapper.Map<StockDto>(stockEntity));
        }
        catch (Exception ex)
        {
            _logger.Here().Error("Failed to update stock {ex}", ex);
            return Result<StockDto>.Faliure(ErrorCodes.OperationFailed);
        }
    }

    private Dictionary<string, string> GetApplicationMetaData()
    {
        return new()
        {
            { "applicationName", _appConfigOptions.ApplicationIdentifier }
        };
    }
}
