using AutoMapper;
using Ecoeden.Inventory.Application.Contracts.Database.SQL;
using Ecoeden.Inventory.Application.Contracts.EventBus;
using Ecoeden.Inventory.Application.Contracts.Resilience;
using Ecoeden.Inventory.Application.Extensions;
using Ecoeden.Inventory.Domain.Entities.SQL;
using Ecoeden.Inventory.Domain.Events;
using Newtonsoft.Json;
using MassTransit;

namespace Ecoeden.Inventory.Infrastructure.EventBus;
public class PublishService<T, TEvent>(IPublishEndpoint publishEndpoint,
    IMapper mapper,
    ILogger logger,
    IUnitOfWork unitOfWork,
    IRetryPolicyService retryPolicyService) : IPublishService<T, TEvent>
    where T : class
    where TEvent : GenericEvent
{
    private readonly IMapper _mapper = mapper;
    private readonly ILogger _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IBaseRepository<EventPublishHistory> _eventPublishRepository = unitOfWork.Repository<EventPublishHistory>();
    private readonly IRetryPolicyService _retryPolicyService = retryPolicyService;

    public async Task PublishAsync(T message, string correlationId, Dictionary<string, string> additionalProperties = null)
    {
        var newEvent = _mapper.Map<TEvent>(message);
        newEvent.CorrelationId = correlationId;
        newEvent.AdditionalProperties = additionalProperties;
        additionalProperties.TryGetValue("applicationName", out string value);
        try
        {
            await AddToEventStorage(newEvent, value);
            await _retryPolicyService.ExecuteAsync(async () => await _publishEndpoint.Publish(newEvent), newEvent.GetType().Name, correlationId);
            _logger.Here()
                .WithCorrelationId(correlationId)
                .Information("Successfully published {messageType} event message", typeof(TEvent).Name);
        }
        catch
        {
            _logger.Here()
               .WithCorrelationId(correlationId)
               .Information("Failed to publish {messageType} event message", typeof(TEvent).Name);
        }
    }

    private async Task AddToEventStorage(TEvent eventname, string applicationName)
    {
        var jsonData = JsonConvert.SerializeObject(eventname);
        EventPublishHistory publishHistory = new()
        {
            CorrelationId = eventname.CorrelationId,
            CreatedAt = eventname.CreatedOn,
            UpdateAt = eventname.LastUpdatedOn,
            Data = jsonData,
            EventStatus = Domain.Models.Enums.EventStatus.Draft,
            EventType = typeof(TEvent).Name,
            FailureSource = applicationName
        };

        _eventPublishRepository.Add(publishHistory);
        await _unitOfWork.Complete();
        _logger.Here().WithCorrelationId(eventname.CorrelationId)
            .Information("Event {Event} added in {Storage} successfulyy", typeof(TEvent).Name, typeof(EventPublishHistory).Name);
    }
}
