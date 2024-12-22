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

namespace Ecoeden.Inventory.Application.Features.Units.Command.UpsertUnit;
public class UpsertUnitCommandHandler(ILogger logger, IMapper mapper, 
    IDocumentRepository<Unit> unitRepository, 
    ICacheServiceBuildFactory cacheServiceFactory, 
    IOptions<AppConfigOption> appConfigOption,
    IPublishServiceFactory publishServiceFactory
) 
    : ICommandHandler<UpsertUnitCommand, Result<UnitDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _appConfigOption = appConfigOption.Value;
    private readonly IDocumentRepository<Unit> _unitRepository = unitRepository;
    private readonly IPublishServiceFactory _publishServiceFactory = publishServiceFactory;
    private readonly ICacheService _cacheService = cacheServiceFactory.CreateService(Domain.Models.Enums.CacheServiceType.Distributed);

    public async Task<Result<UnitDto>> Handle(UpsertUnitCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Request - create or edit unit {name}", request.Unit.Name);

        UnitDto unitDto = new();

        if (string.IsNullOrEmpty(request.Unit.Id))
        {
            var existingUnit = await _unitRepository
                .GetByPredicateAsync(u => u.Name.Equals(request.Unit.Name, StringComparison.CurrentCultureIgnoreCase),
                    MongoDbCollectionNames.Units);

            if(existingUnit is not null)
            {
                _logger.Here().Error("The unit {name} already exists", request.Unit.Name);
                return Result<UnitDto>.Faliure(ErrorCodes.BadRequest, "Unit name already exists");
            }

            unitDto = await CreateUnit(request);
        }
        else
        {
            unitDto = await UpdateUnit(request);
        }

        // cache invalidation
        await _cacheService.RemoveAsync(_appConfigOption.UnitStorageCacheKey, cancellationToken);

        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Unit created/updated successfully");
        _logger.Here().MethodExited();
        return Result<UnitDto>.Success(unitDto);
    }


    private async Task<UnitDto> CreateUnit(UpsertUnitCommand request)
    {
        var unit = _mapper.Map<Unit>(request.Unit);
        var publishService = _publishServiceFactory.CreatePublishService<Unit, UnitCreated>();
        unit.UpdateCreationData(request.RequestInformation.CurrentUser.Id);
        await _unitRepository.UpsertAsync(unit, MongoDbCollectionNames.Units);
        await publishService.PublishAsync(unit, request.RequestInformation.CorrelationId, new()
        {
            { "applicationName", _appConfigOption.ApplicationIdentifier }
        });
        return _mapper.Map<UnitDto>(unit);
    }

    private async Task<UnitDto> UpdateUnit(UpsertUnitCommand request)
    {
        var existingUnit = await _unitRepository.GetByIdAsync(request.Unit.Id, MongoDbCollectionNames.Units);
        var unit = (Unit)_mapper.Map(request.Unit, existingUnit, typeof(UnitDto), typeof(Unit));
        var publishService = _publishServiceFactory.CreatePublishService<Unit, UnitUpdated>();
        unit.UpdateUpdationData(request.RequestInformation.CurrentUser.Id);
        await _unitRepository.UpsertAsync(unit, MongoDbCollectionNames.Units);
        await publishService.PublishAsync(unit, request.RequestInformation.CorrelationId, new()
        {
            { "applicationName", _appConfigOption.ApplicationIdentifier }
        });
        return _mapper.Map<UnitDto>(unit);
    }
}
