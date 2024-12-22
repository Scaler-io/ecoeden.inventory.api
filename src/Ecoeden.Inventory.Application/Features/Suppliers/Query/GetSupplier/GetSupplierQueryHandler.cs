using AutoMapper;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.Repositories;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Application.Features.Suppliers.Query.GetSupplier;
public class GetSupplierQueryHandler(ILogger logger, IDocumentRepository<Supplier> supplierRepository, IMapper mapper) 
    : IQueryHandler<GetSupplierQuery, Result<SupplierDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Supplier> _supplierRepository = supplierRepository;

    public async Task<Result<SupplierDto>> Handle(GetSupplierQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get supplier details by id");

        var supplier = await _supplierRepository.GetByIdAsync(request.Id, MongoDbCollectionNames.Suppliers);
        if(supplier is null)
        {
            _logger.Here().WithSupplierID(request.Id).Error("No supplier has found with id provided");
            return Result<SupplierDto>.Faliure(ErrorCodes.NotFound);
        }

        var supplierDto = _mapper.Map<SupplierDto>(supplier);

        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .WithSupplierID(request.Id)
            .Information("supplier details found {@supplier}", supplier);
        _logger.Here().MethodExited();

        return Result<SupplierDto>.Success(supplierDto);
    }
}
