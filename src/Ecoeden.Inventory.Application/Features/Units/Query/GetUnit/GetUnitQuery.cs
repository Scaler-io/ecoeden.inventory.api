using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Units.Query.GetUnit;
public class GetUnitQuery(string id, RequestInformation requestInformation) : IQuery<Result<UnitDto>>
{
    public string Id { get; set; } = id;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
