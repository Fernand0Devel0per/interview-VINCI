using BuildingBlocks.Messaging.Abstractions;

namespace BuildingBlocks.Messaging.RabbitMq;

public class RabbitMqMessageBus : IMessageBus
{
    private readonly IMessageProducer _producer;
    private readonly IMessageConsumer _consumer;

    public RabbitMqMessageBus(IMessageProducer producer, IMessageConsumer consumer)
    {
        _producer = producer;
        _consumer = consumer;
    }

    public Task PublishAsync<TMessage>(TMessage message, string topic, CancellationToken cancellationToken = default) where TMessage : class
        => _producer.PublishAsync(message, topic, cancellationToken);

    public Task SubscribeAsync<TMessage>(
        string topic,
        Func<TMessage, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default,
        string? queueName = null
    ) where TMessage : class
        => _consumer.SubscribeAsync(topic, handler, cancellationToken, queueName);
}