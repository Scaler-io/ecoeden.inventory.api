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

namespace Ecoeden.Inventory.Application.Features.Customers.Command.DeleteCustomer;
public class DeleteCustomerCommandHandler(ILogger logger,
    ICacheServiceBuildFactory cacheServiceBuildFactory,
    IOptions<AppConfigOption> appConfigOptions,
    IDocumentRepository<Customer> customerRepository,
    IPublishServiceFactory publishServiceFactory
) : ICommandHandler<DeleteCustomerCommand, Result<bool>>
{
    private readonly ILogger _logger = logger;
    private readonly AppConfigOption _appConfigOptions = appConfigOptions.Value;
    private readonly IDocumentRepository<Customer> _customerRepository = customerRepository;
    private readonly ICacheService _cacheService = cacheServiceBuildFactory.CreateService(Domain.Models.Enums.CacheServiceType.Distributed);
    private readonly IPublishService<Customer, CustomerDeleted> _publishService = publishServiceFactory.CreatePublishService<Customer, CustomerDeleted>();

    public async Task<Result<bool>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.Here().MethodEntered();
        _logger.Here().WithCorrelationId(request.RequestInformation.CorrelationId)
            .Information("Request - delete customer {id}", request.Id);

        var isExisting = await _customerRepository.GetByIdAsync(request.Id, MongoDbCollectionNames.Customers);
        if (isExisting == null)
        {
            _logger.Here().WithCustomerID(request.Id).Error("No customer was found");
            return Result<bool>.Faliure(ErrorCodes.NotFound);
        }

        await _customerRepository.DeleteAsync(request.Id, MongoDbCollectionNames.Customers);

        await _publishService.PublishAsync(isExisting, request.RequestInformation.CorrelationId, new()
        {
            { "applicationName", _appConfigOptions.ApplicationIdentifier }
        });

        await _cacheService.RemoveAsync(_appConfigOptions.CustomerStorageCacheKey, cancellationToken);

        _logger.Here().WithCustomerID(request.Id).Information("customer is deleted", request.Id);
        _logger.Here().MethodExited();
        return Result<bool>.Success(true);
    }
}
