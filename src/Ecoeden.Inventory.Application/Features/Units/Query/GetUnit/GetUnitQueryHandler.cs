using AutoMapper;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.Repositories;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Application.Features.Units.Query.GetUnit;
public class GetUnitQueryHandler(ILogger logger, IMapper mapper, IDocumentRepository<Unit> unitRepository) 
    : IQueryHandler<GetUnitQuery, Result<UnitDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Unit> _unitRepository = unitRepository;

    public async Task<Result<UnitDto>> Handle(GetUnitQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Request - get unit details by id");

        var unit = await _unitRepository.GetByIdAsync(request.Id, MongoDbCollectionNames.Units);
        if(unit == null)
        {
            _logger.Here().Error("No unit was found by {id}", request.Id);
            return Result<UnitDto>.Faliure(ErrorCodes.NotFound);
        }

        var unitDto = _mapper.Map<UnitDto>(unit);

        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("unit details fond {id}");
        _logger.Here().MethodExited();

        return Result<UnitDto>.Success(unitDto);
    }
}
