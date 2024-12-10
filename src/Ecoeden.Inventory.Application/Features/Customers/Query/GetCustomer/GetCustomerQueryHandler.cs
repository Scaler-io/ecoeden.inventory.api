using AutoMapper;
using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Application.Contracts.Database.Repositories;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Entities;
using Ecoeden.Inventory.Domain.Models.Constants;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Ecoeden.Inventory.Application.Features.Customers.Query.GetCustomer;
public class GetCustomerQueryHandler(ILogger logger, IMapper mapper, IDocumentRepository<Customer> customerRepository) 
    : IQueryHandler<GetCustomerQuery, Result<CustomerDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IDocumentRepository<Customer> _customerRepository = customerRepository;

    public async Task<Result<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - get customer details by id");

        var customer = await _customerRepository.GetByIdAsync(request.Id, MongoDbCollectionNames.Customers);
        if(customer is null)
        {
            _logger.Here().WithCustomerID(request.Id).Error("No customer was found with id provided");
            return Result<CustomerDto>.Faliure(ErrorCodes.NotFound);
        }

        var customerDto = _mapper.Map<CustomerDto>(customer);

        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .WithCustomerID(request.Id)
            .Information("supplier details found {@supplier}", customer);

        _logger.Here().MethodExited();

        return Result<CustomerDto>.Success(customerDto);
    }
}
