using BuildingBlocks.Core.Enums;
using BuildingBlocks.Core.Events;
using BuildingBlocks.Messaging.Abstractions;

namespace OrderService.CommandAPI.Application.Common;

public class EventPublisherService : IEventPublisherService
{
    private readonly IMessageBus _messageBus;

    public EventPublisherService(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task PublishEntityChangedEventAsync<T>(
        EntityChangeType changeType,
        T data,
        string topic = "entity-changes",
        CancellationToken cancellationToken = default)
    {
        var @event = new EntityChangedEvent<T>(changeType, data);
        await _messageBus.PublishAsync(@event, topic, cancellationToken);
    }
}