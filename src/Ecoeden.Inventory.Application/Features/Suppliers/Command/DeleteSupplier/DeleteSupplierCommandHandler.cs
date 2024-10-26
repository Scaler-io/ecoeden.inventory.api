using Ecoeden.Inventory.Application.Contracts.Caching;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.Repositories;
using Ecoeden.Inventory.Application.Contracts.Factory;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Configurations;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Command.DeleteSupplier;
public class DeleteSupplierCommandHandler(ILogger logger,
    ICacheServiceBuildFactory cacheServiceBuildFactory,
    IOptions<AppConfigOption> appConfigOptions,
    IDocumentRepository<Supplier> supplierRepository) : ICommandHandler<DeleteSupplierCommand, Result<bool>>
{
    private readonly ILogger _logger = logger;
    private readonly ICacheService _cacheService = cacheServiceBuildFactory.CreateService(Domain.Models.Enums.CacheServiceType.Distributed);
    private readonly AppConfigOption _appConfigOptions = appConfigOptions.Value;
    private readonly IDocumentRepository<Supplier> _supplierRepository = supplierRepository;

    public async Task<Result<bool>> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Request - delete supplier {id}", request.Id);

        var isExisting = await _supplierRepository.GetByIdAsync(request.Id, MongoDbCollectionNames.Suppliers);
        if(isExisting == null)
        {
            _logger.Here().WithSupplierID(request.Id).Error("No supplier was found");
            return Result<bool>.Faliure(ErrorCodes.NotFound);
        }

        await _supplierRepository.DeleteAsync(request.Id, MongoDbCollectionNames.Suppliers);

        // cache invalidation
        await _cacheService.RemoveAsync(_appConfigOptions.SupplierStorageCacheKey, cancellationToken);

        _logger.Here().WithSupplierID(request.Id).Information("supplier is deleted", request.Id);
        _logger.Here().MethodExited();
        return Result<bool>.Success(true);
    }
}
