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

namespace Ecoeden.Inventory.Application.Features.Units.Query.ListUnit;
public class ListUnitQueryHandler(ILogger logger, 
    IMapper mapper, 
    IOptions<AppConfigOption> appConfigOption, 
    ICacheServiceBuildFactory cacheServiceFactory, 
    IDocumentRepository<Unit> unitRepository) : IQueryHandler<ListUnitQuery, Result<IReadOnlyList<UnitDto>>>
{
    private readonly ILogger _logger = logger;
    private readonly AppConfigOption _appConfigOption = appConfigOption.Value;
    private readonly ICacheService _cacheService = cacheServiceFactory.CreateService(Domain.Models.Enums.CacheServiceType.Distributed);
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Unit> _unitRepository = unitRepository;

    public async Task<Result<IReadOnlyList<UnitDto>>> Handle(ListUnitQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - List all units");

       var cacheResult = await _cacheService.GetAsync<IReadOnlyList<UnitDto>>(_appConfigOption.UnitStorageCacheKey, cancellationToken);

        if(cacheResult != null && cacheResult.Count != 0)
        {
            _logger.Here().Information("ListUnits - cache hit");
            return Result<IReadOnlyList<UnitDto>>.Success(cacheResult);
        }

        _logger.Here().Information("ListUnits- cache miss");
        var units = await _unitRepository.GetAllAsync(MongoDbCollectionNames.Units);

        if(units == null || units.Count == 0)
        {
            _logger.Here().Error("No units found in database");
            return Result<IReadOnlyList<UnitDto>>.Faliure(ErrorCodes.NotFound);
        }

        var result = _mapper.Map<IReadOnlyList<UnitDto>>(units);
        await _cacheService.SetAsync(_appConfigOption.UnitStorageCacheKey, result, cancellation: cancellationToken);

        _logger.Here().Information("Total {count} units fetched", units.Count);
        _logger.Here().MethodExited();
        return Result<IReadOnlyList<UnitDto>>.Success(result);
    }
}
