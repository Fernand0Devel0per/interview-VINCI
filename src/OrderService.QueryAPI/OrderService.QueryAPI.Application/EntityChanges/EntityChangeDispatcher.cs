using BuildingBlocks.Core.Enums;
using OrderService.QueryAPI.Application.Common.Abstractions;

namespace OrderService.QueryAPI.Application.EntityChanges;

public class EntityChangeDispatcher : IEntityChangeDispatcher
{
    private readonly CustomerChangeHandler _customerHandler;
    private readonly ProductChangeHandler _productHandler;
    private readonly OrderChangeHandler _orderHandler;

    public EntityChangeDispatcher(
        CustomerChangeHandler customerHandler,
        ProductChangeHandler productHandler,
        OrderChangeHandler orderHandler)
    {
        _customerHandler = customerHandler;
        _productHandler = productHandler;
        _orderHandler = orderHandler;
    }

    public async Task DispatchAsync(string entityType, string rawData, EntityChangeType changeType, CancellationToken cancellationToken)
    {
        switch (entityType)
        {
            case "Customer":
                await _customerHandler.HandleAsync(rawData, changeType, cancellationToken);
                break;
            case "Product":
                await _productHandler.HandleAsync(rawData, changeType, cancellationToken);
                break;
            case "Order":
                await _orderHandler.HandleAsync(rawData, changeType, cancellationToken);
                break;
            default:
                Console.WriteLine($"[!] Unknown entity type: {entityType}");
                break;
        }
    }
}