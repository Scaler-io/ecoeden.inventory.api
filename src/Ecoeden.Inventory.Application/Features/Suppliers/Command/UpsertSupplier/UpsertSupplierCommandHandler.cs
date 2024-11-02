using AutoMapper;
using Contracts.Events;
using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.Repositories;
using Ecoeden.Inventory.Application.Contracts.EventBus;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Command.UpsertSupplier;
public class UpsertSupplierCommandHandler(ILogger logger,
    IMapper mapper,
    IOptions<AppConfigOption> appConfigOptions,
    IDocumentRepository<Supplier> supplierRepository,
    ICacheServiceBuildFactory cacheServiceBuildFactory,
    IPublishServiceFactory publishServiceFactory
) : ICommandHandler<UpsertSupplierCommand, Result<SupplierDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _appConfigOptions = appConfigOptions.Value;
    private readonly IDocumentRepository<Supplier> _supplierRepository = supplierRepository;
    private readonly ICacheService _cacheService = cacheServiceBuildFactory.CreateService(CacheServiceType.Distributed);
    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;

    public async Task<Result<SupplierDto>> Handle(UpsertSupplierCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Request - create or update supplier {name}", request.Supplier.Name);

        SupplierDto supplierDto = new();
        

        if (string.IsNullOrEmpty(request.Supplier.Id))
        {
            var existingSupplier = await _supplierRepository.GetByPredicateAsync(supplier => supplier.Name.Equals(request.Supplier.Name, StringComparison.CurrentCultureIgnoreCase),
            MongoDbCollectionNames.Suppliers);

            if (existingSupplier is not null)
            {
                _logger.Here().Error("The supplier {name} already exists", request.Supplier.Name);
                return Result<SupplierDto>.Faliure(ErrorCodes.BadRequest, "Supplier name already exists");
            }
            supplierDto = await CreateSupplier(request);
        }
        else
        {
            supplierDto = await UpdateSupplier(request);
        }

        // cache invalidation
        await _cacheService.RemoveAsync(_appConfigOptions.SupplierStorageCacheKey, cancellationToken);
        _logger.Here().WithSupplierID(supplierDto.Id).Information("Supplier created/updated successfully");
        _logger.Here().MethodExited();

        return Result<SupplierDto>.Success(supplierDto);
    }

    private async Task<SupplierDto> CreateSupplier(UpsertSupplierCommand request)
    {
        var supplier = _mapper.Map<Supplier>(request.Supplier);

        var publishService = _publishServiceFactory.CreatePublishService<Supplier, SupplierCreated>();
        supplier.UpdateCreationData(request.RequestInformation.CurrentUser.Id);

        await _supplierRepository.UpsertAsync(supplier, MongoDbCollectionNames.Suppliers);

        await publishService.PublishAsync(supplier, request.RequestInformation.CorrelationId, new()
        {
            { "applicationName", _appConfigOptions.ApplicationIdentifier }
        });

        return _mapper.Map<SupplierDto>(supplier);
    }

    private async Task<SupplierDto> UpdateSupplier(UpsertSupplierCommand request)
    {
        var existingSupplier = await _supplierRepository.GetByIdAsync(request.Supplier.Id, MongoDbCollectionNames.Suppliers);
        var publishService = _publishServiceFactory.CreatePublishService<Supplier, SupplierUpdated>();

        var supplier = (Supplier)_mapper.Map(request.Supplier, existingSupplier, typeof(SupplierDto), typeof(Supplier));
        supplier.UpdateUpdationData(request.RequestInformation.CurrentUser.Id);

        await _supplierRepository.UpsertAsync(supplier, MongoDbCollectionNames.Suppliers);

        await publishService.PublishAsync(supplier, request.RequestInformation.CorrelationId, new()
        {
            { "applicationName", _appConfigOptions.ApplicationIdentifier }
        });

        return _mapper.Map<SupplierDto>(supplier);
    }
}
