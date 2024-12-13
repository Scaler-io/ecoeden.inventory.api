using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Attributes;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Customers.Command.UpsertCustomer;
public class UpsertCustomerCommand(CustomerDto customer, RequestInformation requestInformation) : ICommand<Result<CustomerDto>>
{
    [ValidateNested]
    public CustomerDto Customer { get; set; } = customer;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
