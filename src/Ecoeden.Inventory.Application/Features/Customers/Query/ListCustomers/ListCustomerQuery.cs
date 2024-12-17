using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Customers.Query.ListCustomers;
public class ListCustomerQuery : IQuery<Result<IReadOnlyList<CustomerDto>>>
{
}
