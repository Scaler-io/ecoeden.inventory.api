using Ecoeden.Inventory.Application.Contracts.Resilience;
using Ecoeden.Inventory.Application.Extensions;
using Polly;

namespace Ecoeden.Inventory.Infrastructure.Resilience;
public class RetryPolicyService(ILogger logger) : IRetryPolicyService
{
    private readonly ILogger _logger = logger;

    public async Task ExecuteAsync(Func<Task> action, string operationName, string correlationId)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttemt => TimeSpan.FromSeconds(Math.Pow(2, retryAttemt)),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    _logger.Here().Error("Retry {RetryCount} for process {CorrelationID} after {TimeSpan} due to {ExceptionType}", retryCount, correlationId, timespan, exception.GetType().Name);
                });

        await retryPolicy.ExecuteAsync(action);
    }
}
