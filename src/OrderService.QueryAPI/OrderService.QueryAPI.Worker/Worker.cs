using System.Text;
using System.Text.Json;
using BuildingBlocks.Messaging.Abstractions;
using OrderService.CommandAPI.Application.Common;
using OrderService.QueryAPI.Application.Common.Abstractions;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace OrderService.QueryAPI.Worker;

public class Worker : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly IEntityChangeDispatcher _dispatcher;
    private readonly IChannel _channel;

    public Worker(IMessageBus messageBus, IEntityChangeDispatcher dispatcher, IChannel  channel)
    {
        _messageBus = messageBus;
        _dispatcher = dispatcher;
        _channel = channel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[*] Worker started, listening for messages...");

        await _channel.BasicQosAsync(0, 1, false);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var messageJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                var entityChangedEvent = JsonSerializer.Deserialize<EntityChangedEvent<JsonElement>>(messageJson);

                if (entityChangedEvent is null)
                {
                    Console.WriteLine("[!] Failed to deserialize EntityChangedEvent.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
                    return;
                }

                await _dispatcher.DispatchAsync(
                    entityChangedEvent.EntityType,
                    entityChangedEvent.Data.GetRawText(),
                    entityChangedEvent.ChangeType,
                    stoppingToken
                );

                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Error processing message: {ex.Message}");

                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await _channel.BasicConsumeAsync(queue: "entity-changes-queue", autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
