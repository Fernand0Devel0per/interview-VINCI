using System.Text.Json;
using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.Enums;
using OrderService.QueryAPI.Application.Common.Abstractions;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.EntityChanges;

public class CustomerChangeHandler : IEntityChangeHandler
{
    private readonly ICustomerMongoRepository _customerRepository;
    private readonly ICacheService _cacheService;

    public CustomerChangeHandler(ICustomerMongoRepository customerRepository, ICacheService cacheService)
    {
        _customerRepository = customerRepository;
        _cacheService = cacheService;
    }

    public async Task HandleAsync(string rawData, EntityChangeType changeType, CancellationToken cancellationToken)
    {
        var customer = JsonSerializer.Deserialize<Customer>(rawData);

        if (customer is null)
            throw new Exception("Failed to deserialize Customer.");

        switch (changeType)
        {
            case EntityChangeType.Created:
            case EntityChangeType.Updated:
                await _customerRepository.UpsertAsync(customer, cancellationToken);
                break;
            case EntityChangeType.Deleted:
                await _customerRepository.DeleteAsync(customer.Id, cancellationToken);
                break;
        }
        
        await _cacheService.RemoveAsync($"customer:{customer.Id}", cancellationToken);
    }
}