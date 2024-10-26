using Ecoeden.Inventory.Domain.Attributes;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ecoeden.Inventory.Application.Behaviors;
public class ValidationBehavior<TRequest, TResponse>(IServiceProvider serviceProvider) : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validationFailures = new List<ValidationFailure>();

        // Validate top-level request
        var requestValidator = _serviceProvider.GetService<IValidator<TRequest>>();
        if (requestValidator != null) 
        {
            var context = new ValidationContext<TRequest>(request);
            var result = await requestValidator.ValidateAsync(context, cancellationToken);
            validationFailures.AddRange(result.Errors);
        }

        // Validate nested properties
        validationFailures.AddRange(ValidateNestedProperties(request, cancellationToken));
        if (validationFailures.Count != 0)
        {
            throw new ValidationException(validationFailures);
        }

        return await next();
    }

    private IEnumerable<ValidationFailure> ValidateNestedProperties(object request, CancellationToken cancellationToken)
    {
        var failures = new List<ValidationFailure>();
        var properties = request.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (!property.IsDefined(typeof(ValidateNestedAttribute), inherit: true))
            {
                continue;
            }

            var propertyValue = property.GetValue(request);
            if (propertyValue == null) continue;

            var propertyType = property.PropertyType;
            var validatorType = typeof(IValidator<>).MakeGenericType(propertyType);
            var validators = (IEnumerable<object>)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(validatorType));

            if (validators != null)
            {
                var context = new ValidationContext<object>(propertyValue);
                foreach (var validator in validators)
                {
                    var validationResults = ((IValidator)validator).ValidateAsync(context, cancellationToken).Result;
                    failures.AddRange(validationResults.Errors);
                }
            }
        }

        return failures;
    }
}
