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

namespace Ecoeden.Inventory.Application.Features.Customers.Query.ListCustomers;
public class ListCustomersQueryHandler(ILogger logger, 
    IMapper mapper, 
    ICacheServiceBuildFactory cacheServiceFactory, 
    IDocumentRepository<Customer> customerRepository, 
    IOptions<AppConfigOption> appConfigOptions
) : IQueryHandler<ListCustomerQuery, Result<IReadOnlyList<CustomerDto>>>
{
    private readonly ILogger _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly ICacheService _cacheService = cacheServiceFactory.CreateService(CacheServiceType.Distributed);
    private readonly IDocumentRepository<Customer> _customerRepository = customerRepository;
    private readonly AppConfigOption _appConfigOptions = appConfigOptions.Value;

    public async Task<Result<IReadOnlyList<CustomerDto>>> Handle(ListCustomerQuery request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().Information("Request - list all customers");

        var cacheResult = await _cacheService.GetAsync<IReadOnlyList<CustomerDto>>(_appConfigOptions.CustomerStorageCacheKey, cancellationToken);
        if(cacheResult is not null && cacheResult.Count != 0)
        {
            _logger.Here().Information("ListCustomers - cache hit");
            return Result<IReadOnlyList<CustomerDto>>.Success(cacheResult);
        }

        _logger.Here().Information("ListCustomers - cache miss");
        var customers = await _customerRepository.GetAllAsync(MongoDbCollectionNames.Customers);

        if(customers == null || customers.Count == 0)
        {
            _logger.Here().Error("No cstomer was found");
            return Result<IReadOnlyList<CustomerDto>>.Faliure(ErrorCodes.NotFound);
        }

        var result = _mapper.Map<IReadOnlyList<CustomerDto>>(customers);
        await _cacheService.SetAsync(_appConfigOptions.CustomerStorageCacheKey, result, cancellation: cancellationToken);

        _logger.Here().Information("Total {count} customers details fetched", customers.Count);
        _logger.Here().MethodExited();

        return Result<IReadOnlyList<CustomerDto>>.Success(result);
    }
}
