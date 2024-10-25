using AutoMapper;
using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.Repositories;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Query.ListSuppliers;
public class ListSuppliersQueryHandler(ILogger logger, 
    IMapper mapper, 
    IDocumentRepository<Supplier> supplierRepository, 
    IOptions<AppConfigOption> appConfigOptions,
    ICacheServiceBuildFactory cacheServiceBuildFactory
) : IQueryHandler<ListSuppliersQuery, Result<IReadOnlyList<SupplierDto>>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ICacheService _cacheService = cacheServiceBuildFactory.CreateService(CacheServiceType.Distributed);
    private readonly IDocumentRepository<Supplier> _supplierRepository = supplierRepository;
    private readonly AppConfigOption _appConfigOptions = appConfigOptions.Value;

    public async Task<Result<IReadOnlyList<SupplierDto>>> Handle(ListSuppliersQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here()
            .WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Request - list all suppliers");
        
        var cacheResult = await _cacheService.GetAsync<IReadOnlyList<SupplierDto>>(_appConfigOptions.SupplierStorageCacheKey, cancellationToken);
        if (cacheResult is not null && cacheResult.Count != 0) 
        {
            _logger.Here().Information("ListSuppliers - cache hit");
            return Result<IReadOnlyList<SupplierDto>>.Success(cacheResult);
        }

        _logger.Here().Information("ListSuppliers - cache miss");
        var suppliers = await _supplierRepository.GetAllAsync(MongoDbCollectionNames.Suppliers);

        if(suppliers == null || suppliers.Count == 0)
        {
            _logger.Here().Error("No supplier was found");
            return Result<IReadOnlyList<SupplierDto>>.Faliure(ErrorCodes.NotFound);
        }

        var result = _mapper.Map<IReadOnlyList<SupplierDto>>(suppliers);
        await _cacheService.SetAsync(_appConfigOptions.SupplierStorageCacheKey, result, cancellation: cancellationToken);

        _logger.Here()
            .WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Total {count} supplier details fetched", suppliers.Count);
        _logger.Here().MethodExited();

        return Result<IReadOnlyList<SupplierDto>>.Success(result);
    }
}
