namespace BuildingBlocks.Messaging.Abstractions;

public interface IMessageProducer
{
    Task PublishAsync<TMessage>(TMessage message, string topic, CancellationToken cancellationToken = default) where TMessage : class;
}