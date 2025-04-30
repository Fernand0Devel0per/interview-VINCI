using BuildingBlocks.Core.Enums;

namespace OrderService.CommandAPI.Application.Common;

public interface IEventPublisherService
{
    Task PublishEntityChangedEventAsync<T>(
        EntityChangeType changeType,
        T data,
        string topic = "entity-changes",
        CancellationToken cancellationToken = default);
}