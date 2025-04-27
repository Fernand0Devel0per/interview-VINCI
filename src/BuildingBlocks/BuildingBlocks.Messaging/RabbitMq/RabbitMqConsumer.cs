using System.Text;
using System.Text.Json;
using BuildingBlocks.Messaging.Abstractions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BuildingBlocks.Messaging.RabbitMq;

public class RabbitMqConsumer : IMessageConsumer, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqConsumer(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };

        _connection = factory.CreateConnectionAsync().Result;
        _channel = _connection.CreateChannelAsync().Result;
    }

    public async Task SubscribeAsync<TMessage>(string topic, Func<TMessage, CancellationToken, Task> handler, CancellationToken cancellationToken = default) where TMessage : class
    {
        if (_channel is null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");

        await _channel.ExchangeDeclareAsync(exchange: topic, type: ExchangeType.Fanout, durable: true, autoDelete: false);

        var queueName = (await _channel.QueueDeclareAsync()).QueueName;
        await _channel.QueueBindAsync(queue: queueName, exchange: topic, routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (object? sender, BasicDeliverEventArgs ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<TMessage>(messageJson);

                if (message is not null)
                    await handler(message, cancellationToken);

                await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error processing message: {ex.Message}");
                await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer
        );

        Console.WriteLine($"[*] Waiting for messages on topic {topic}.");
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
