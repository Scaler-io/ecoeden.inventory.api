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
using Microsoft.Extensions.Options;

namespace Ecoeden.Inventory.Application.Features.Customers.Command.UpsertCustomer;
public class UpsertCustomerCommandHandler(ILogger logger, 
    IMapper mapper, 
    IOptions<AppConfigOption> appConfigOptions, 
    ICacheServiceBuildFactory cacheServiceFactory, 
    IDocumentRepository<Customer> customerRepository
) : ICommandHandler<UpsertCustomerCommand, Result<CustomerDto>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly AppConfigOption _appConfigOptions = appConfigOptions.Value;
    private readonly IDocumentRepository<Customer> _customerRepository = customerRepository;
    private readonly ICacheService _cacheService = cacheServiceFactory.CreateService(Domain.Models.Enums.CacheServiceType.Distributed);

    public async Task<Result<CustomerDto>> Handle(UpsertCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here()
            .WithCustomerID(request.RequestInformation.CorrelationId)
            .Information("Request - create or delete customer {name}", request.Customer.Name);

        CustomerDto customerDto = new();

        if(string.IsNullOrEmpty(request.Customer.Id))
        {
            customerDto = await CreateCustomer(request);
        }
        else
        {
            customerDto = await UpdateCustomer(request);
        }

        await _cacheService.RemoveAsync(_appConfigOptions.CustomerStorageCacheKey, cancellationToken);
        _logger.Here().WithCustomerID(customerDto.Id).Information("Customer created/updated successfully");
        _logger.Here().MethodExited();

        return Result<CustomerDto>.Success(customerDto);
    }

    private async Task<CustomerDto> CreateCustomer(UpsertCustomerCommand request)
    {
        var customer = _mapper.Map<Customer>(request.Customer);

        //var publishService = _publishServiceFactory.CreatePublishService<Supplier, SupplierCreated>();
        //supplier.UpdateCreationData(request.RequestInformation.CurrentUser.Id);

        customer.UpdateCreationData(request.RequestInformation.CurrentUser.Id);
        await _customerRepository.UpsertAsync(customer, MongoDbCollectionNames.Customers);

        //await publishService.PublishAsync(supplier, request.RequestInformation.CorrelationId, new()
        //{
        //    { "applicationName", _appConfigOptions.ApplicationIdentifier }
        //});

        return _mapper.Map<CustomerDto>(customer);
    }

    private async Task<CustomerDto> UpdateCustomer(UpsertCustomerCommand request)
    {
        var existingCustomer = await _customerRepository.GetByIdAsync(request.Customer.Id, MongoDbCollectionNames.Customers);

        //var publishService = _publishServiceFactory.CreatePublishService<Supplier, SupplierUpdated>();

        var customer = (Customer)_mapper.Map(request.Customer, existingCustomer, typeof(CustomerDto), typeof(Customer));
        customer.UpdateUpdationData(request.RequestInformation.CurrentUser.Id);

        await _customerRepository.UpsertAsync(customer, MongoDbCollectionNames.Customers);

        //await publishService.PublishAsync(customer, request.RequestInformation.CorrelationId, new()
        //{
        //    { "applicationName", _appConfigOptions.ApplicationIdentifier }
        //});

        return _mapper.Map<CustomerDto>(customer);
    }
}
