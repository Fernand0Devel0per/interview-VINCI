using System.Text.Json;
using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.Enums;
using OrderService.QueryAPI.Application.Common.Abstractions;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.EntityChanges;

public class OrderChangeHandler : IEntityChangeHandler
{
    private readonly IOrderMongoRepository _orderRepository;
    private readonly ICacheService _cacheService;

    public OrderChangeHandler(IOrderMongoRepository orderRepository, ICacheService cacheService)
    {
        _orderRepository = orderRepository;
        _cacheService = cacheService;
    }

    public async Task HandleAsync(string rawData, EntityChangeType changeType, CancellationToken cancellationToken)
    {
        var order = JsonSerializer.Deserialize<Order>(rawData);

        if (order is null)
            throw new Exception("Failed to deserialize Order.");

        switch (changeType)
        {
            case EntityChangeType.Created:
            case EntityChangeType.Updated:
                await _orderRepository.UpsertAsync(order, cancellationToken);
                break;

            case EntityChangeType.Deleted:
                await _orderRepository.DeleteAsync(order.Id, cancellationToken);
                break;
        }
        
        await _cacheService.RemoveAsync($"order:{order.Id}", cancellationToken);
    }
}