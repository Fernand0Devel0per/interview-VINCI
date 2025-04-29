using System.Text.Json;
using BuildingBlocks.Core.Enums;
using OrderService.QueryAPI.Application.Common.Abstractions;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.EntityChanges;

public class OrderChangeHandler : IEntityChangeHandler
{
    private readonly IOrderMongoRepository _orderRepository;

    public OrderChangeHandler(IOrderMongoRepository orderRepository)
    {
        _orderRepository = orderRepository;
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
    }
}