using System.Text.Json;
using BuildingBlocks.Core.Enums;
using OrderService.QueryAPI.Application.Common.Abstractions;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.EntityChanges;

public class CustomerChangeHandler : IEntityChangeHandler
{
    private readonly ICustomerMongoRepository _customerRepository;

    public CustomerChangeHandler(ICustomerMongoRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(string rawData, EntityChangeType changeType, CancellationToken cancellationToken)
    {
        var customer = JsonSerializer.Deserialize<Customer>(rawData);

        if (customer is null)
            throw new Exception("Failed to deserialize Customer.");

        switch (changeType)
        {
            case EntityChangeType.Created:
                await _customerRepository.AddAsync(customer, cancellationToken);
                break;
            case EntityChangeType.Updated:
                await _customerRepository.UpdateAsync(customer.Id, customer, cancellationToken);
                break;
            case EntityChangeType.Deleted:
                await _customerRepository.DeleteAsync(customer.Id, cancellationToken);
                break;
        }
    }
}