﻿using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;
using Ecoeden.Inventory.Domain.Models.Dtos;

namespace Ecoeden.Inventory.Application.Features.Units.Query.ListUnit;
public class ListUnitQuery() : IQuery<Result<IReadOnlyList<UnitDto>>>
{
}
