using Ecoeden.Inventory.Domain.Models.Constants;
using Serilog;
using System.Runtime.CompilerServices;

namespace Ecoeden.Inventory.Application.Extensions;
public static class LoggerExtensions
{
    public static ILogger Here(this ILogger logger,
        [CallerMemberName] string member = "",
        [CallerFilePath] string caller = "")
    {
        var callerType = Path.GetFileNameWithoutExtension(caller);
        return logger.ForContext(LoggerConstants.MemberName, member)
            .ForContext(LoggerConstants.CallerType, callerType);
    }

    public static void MethodEntered(this ILogger logger)
    {
        logger.Debug(LoggerConstants.MethodEntered);
    }

    public static void MethodExited(this ILogger logger, object? withResult = null)
    {
        logger.Debug(LoggerConstants.MethodExited);
        if (withResult is not null) logger.Debug("{MethodExited} with result {@result}", LoggerConstants.MethodExited, withResult);
    }

    public static ILogger WithCorrelationId(this ILogger logger, string correlationId)
    {
        return logger.ForContext(LoggerConstants.CorrelationId, correlationId);
    }
    public static ILogger WithSupplierID(this ILogger logger, string supplierId)
    {
        return logger.ForContext(LoggerConstants.SupplierId, supplierId);
    }
    public static ILogger WithCustomerID(this ILogger logger, string customerId)
    {
        return logger.ForContext(LoggerConstants.SupplierId, customerId);
    }
}
