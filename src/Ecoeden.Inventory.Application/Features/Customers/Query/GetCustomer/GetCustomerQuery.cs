using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Customers.Query.GetCustomer;
public class GetCustomerQuery(string id, RequestInformation requestInformation) : IQuery<Result<CustomerDto>>
{
    public string Id { get; set; } = id;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
