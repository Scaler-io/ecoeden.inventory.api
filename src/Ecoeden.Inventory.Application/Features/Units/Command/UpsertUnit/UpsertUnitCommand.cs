using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Attributes;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Units.Command.UpsertUnit;
public class UpsertUnitCommand(UnitDto unit, RequestInformation requestInformation) 
    : ICommand<Result<UnitDto>>
{
    [ValidateNested]
    public UnitDto Unit { get; set; } = unit;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
