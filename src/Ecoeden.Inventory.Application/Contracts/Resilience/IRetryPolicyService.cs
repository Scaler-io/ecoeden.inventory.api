namespace Ecoeden.Inventory.Application.Contracts.Resilience;
public interface IRetryPolicyService
{
    Task ExecuteAsync(Func<Task> action, string operationName, string correlationId);
}
