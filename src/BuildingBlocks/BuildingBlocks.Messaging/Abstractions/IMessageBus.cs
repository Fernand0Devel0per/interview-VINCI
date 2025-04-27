namespace BuildingBlocks.Messaging.Abstractions;

public interface IMessageBus
{
    Task PublishAsync<TMessage>(TMessage message, string topic, CancellationToken cancellationToken = default) where TMessage : class;
    Task SubscribeAsync<TMessage>(string topic, Func<TMessage, CancellationToken, Task> handler, CancellationToken cancellationToken = default) where TMessage : class;
}