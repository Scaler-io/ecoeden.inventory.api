﻿using Ecoeden.Inventory.Domain.Events;
using Ecoeden.Inventory.Domain.Models.Enums;

namespace Contracts.Events;
public class UnitUpdated : GenericEvent
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }
    protected override GenericEventType GenericEventType { get; set; } = GenericEventType.UnitUpdated;
}
