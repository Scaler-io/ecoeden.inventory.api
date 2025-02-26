﻿using Ecoeden.Inventory.Application.Contracts.CQRS;
using Ecoeden.Inventory.Domain.Models.Core;

namespace Ecoeden.Inventory.Application.Features.Customers.Command.DeleteCustomer;
public class DeleteCustomerCommand(string id, RequestInformation requestInformation) : ICommand<Result<bool>>
{
    public string Id { get; set; } = id;
    public RequestInformation RequestInformation { get; set; } = requestInformation;
}
