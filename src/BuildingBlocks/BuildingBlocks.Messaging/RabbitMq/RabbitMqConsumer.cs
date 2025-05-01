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
        (_connection, _channel) = CreateConnectionWithRetry(hostName, userName, password);

    }
    
    private (IConnection connection, IChannel channel) CreateConnectionWithRetry(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password
        };

        const int maxAttempts = 5;
        int attempt = 0;
        Exception? lastException = null;

        while (attempt < maxAttempts)
        {
            try
            {
                var connection = factory.CreateConnectionAsync().Result;
                var channel = connection.CreateChannelAsync().Result;
                return (connection, channel);
            }
            catch (Exception ex)
            {
                attempt++;
                lastException = ex;
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        throw new Exception("Could not connect to RabbitMQ after multiple attempts", lastException);
    }

    public async Task SubscribeAsync<TMessage>(
        string topic,
        Func<TMessage, CancellationToken, Task> handler,
        CancellationToken cancellationToken = default,
        string? queueName = null
    ) where TMessage : class
    {
        if (_channel is null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");
        
        await _channel.ExchangeDeclareAsync(exchange: topic, type: ExchangeType.Fanout, durable: true, autoDelete: false);
        
        queueName ??= topic;
        
        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        
        await _channel.QueueBindAsync(
            queue: queueName,
            exchange: topic,
            routingKey: string.Empty
        );

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

        Console.WriteLine($"[*] Waiting for messages on exchange '{topic}' and queue '{queueName}'.");
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
