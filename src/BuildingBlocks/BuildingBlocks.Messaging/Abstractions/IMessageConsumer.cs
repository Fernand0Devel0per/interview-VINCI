namespace BuildingBlocks.Messaging.Abstractions;

public interface IMessageConsumer
{
 Task SubscribeAsync<TMessage>(
        string topic,
        Func<TMessage, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default,
        string? queueName = null
    ) where TMessage : class;
}