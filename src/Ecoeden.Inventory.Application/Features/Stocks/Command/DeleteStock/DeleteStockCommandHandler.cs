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

namespace Ecoeden.Inventory.Application.Features.Stocks.Command.DeleteStock;
public class DeleteStockCommandHandler(ILogger logger,
    IUnitOfWorkFactory unitOfWorkFactory,
    IOptions<AppConfigOption> options,
    ICacheServiceBuildFactory cacheServiceBuildFactory,
    IPublishServiceFactory publishServiceFactory,
    IMapper mapper) : ICommandHandler<DeleteStockCommand, Result<bool>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _appConfigOptions = options.Value;
    private readonly IUnitOfWork _unitOfWork = unitOfWorkFactory.CreateUnitOfWork("ecoedenStock");
    private readonly ICacheService _cacheService = cacheServiceBuildFactory.CreateService(CacheServiceType.Distributed);
    private readonly IBaseRepository<ProductStock> _stockRepository = unitOfWorkFactory.CreateUnitOfWork("ecoedenStock").Repository<ProductStock>();
    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;

    public async Task<Result<bool>> Handle(DeleteStockCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId).Information("Request - delete stock {0} {1}", request.ProductId, request.SupplierId);

        var stockEntity = await _stockRepository.GetEntityByPredicate(s => s.ProductId == request.ProductId && s.SupplierId == request.SupplierId);
        if (stockEntity == null)
        {
            _logger.Here().Error("No product was found with product {0} and supplier {1}", request.ProductId, request.SupplierId);
            return Result<bool>.Faliure(ErrorCodes.NotFound);
        }

        _stockRepository.Delete(stockEntity);
        var deleteResponse = await _unitOfWork.Complete();

        if(deleteResponse < 1)
        {
            _logger.Here().Error("Failed to delet stock {id}", stockEntity.Id);
            return Result<bool>.Faliure(ErrorCodes.OperationFailed);
        }

        await _cacheService.RemoveAsync(_appConfigOptions.StockStorageCacheKey, cancellationToken);
        var stockDeletePublishService = _publishServiceFactory.CreatePublishService<ProductStock, ProductStockDeleted>();
        await stockDeletePublishService.PublishAsync(stockEntity, request.RequestInformation.CorrelationId, GetApplicationMetaData());

        _logger.Here().Information("Stock {id} deleted successfully");
        _logger.Here().MethodExited();

        return Result<bool>.Success(true);
    }

    private Dictionary<string, string> GetApplicationMetaData()
    {
        return new()
        {
            { "applicationName", _appConfigOptions.ApplicationIdentifier }
        };
    }
}
