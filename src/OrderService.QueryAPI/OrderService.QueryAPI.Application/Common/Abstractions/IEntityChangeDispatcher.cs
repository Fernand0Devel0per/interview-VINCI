using BuildingBlocks.Core.Enums;

namespace OrderService.QueryAPI.Application.Common.Abstractions;

public interface IEntityChangeDispatcher
{
    Task DispatchAsync(string entityType, string rawData, EntityChangeType changeType, CancellationToken cancellationToken);
}