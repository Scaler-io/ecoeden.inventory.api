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
using Ecoeden.Inventory.Domain.Models.Enums;
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Application.Features.Units.Command.DeleteUnit;
public class DeleteUnitCommandHandler(ILogger logger, 
    ICacheServiceBuildFactory cacheServiceFactory,
    IOptions<AppConfigOption> appConfigOption,
    IDocumentRepository<Unit> unitReository,
    IPublishServiceFactory publishServiceFactory) : ICommandHandler<DeleteUnitCommand, Result<bool>>
{
    private readonly ILogger _logger = logger;
    private readonly IDocumentRepository<Unit> _unitReository = unitReository;
    private readonly AppConfigOption _appConfigOption = appConfigOption.Value;
    private readonly IPublishService<Unit, UnitDeleted> _publishService = publishServiceFactory.CreatePublishService<Unit, UnitDeleted>();
    private readonly ICacheService _cacheService = cacheServiceFactory.CreateService(Domain.Models.Enums.CacheServiceType.Distributed);

    public async Task<Result<bool>> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Request - delete unit {id}", request.Id);

        var isExisting = await _unitReository.GetByIdAsync(request.Id, MongoDbCollectionNames.Units);
        if (isExisting == null)
        {
            _logger.Here().Error("No unit was found");
            return Result<bool>.Faliure(ErrorCodes.NotFound);
        }

        await _unitReository.DeleteAsync(request.Id, MongoDbCollectionNames.Units);
        await _publishService.PublishAsync(isExisting, request.RequestInformation.CorrelationId, new()
        {
            { "applicationName", _appConfigOption.ApplicationIdentifier }
        });

        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("unit is deleted", request.Id);
        _logger.Here().MethodExited();
        return Result<bool>.Success(true);
    }
}
