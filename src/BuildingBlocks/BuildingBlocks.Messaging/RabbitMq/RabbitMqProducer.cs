using System.Text;
using System.Text.Json;
using BuildingBlocks.Messaging.Abstractions;
using RabbitMQ.Client;

namespace BuildingBlocks.Messaging.RabbitMq;

public class RabbitMqProducer : IMessageProducer, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqProducer(string hostName, string userName, string password)
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

    public async Task PublishAsync<TMessage>(
        TMessage message,
        string topic,
        CancellationToken cancellationToken = default
    ) where TMessage : class
    {
        if (_channel is null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized.");

        await _channel.ExchangeDeclareAsync(
            exchange: topic,
            type: ExchangeType.Fanout,
            durable: true,
            autoDelete: false
        );

        await _channel.QueueDeclareAsync(
            queue: topic,
            durable: true,
            exclusive: false,
            autoDelete: false
        );


        await _channel.QueueBindAsync(
            queue: topic,
            exchange: topic,
            routingKey: string.Empty
        );
        
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await _channel.BasicPublishAsync(
            exchange: topic,
            routingKey: string.Empty,
            body: body
        );
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.DisposeAsync();
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
