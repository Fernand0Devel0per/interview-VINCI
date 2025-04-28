using System.Text.Json;
using BuildingBlocks.Messaging.Abstractions;
using OrderService.CommandAPI.Application.Common;
using OrderService.QueryAPI.Application.Common.Abstractions;
using Microsoft.Extensions.Configuration;

namespace OrderService.QueryAPI.Worker;

public class Worker : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly IEntityChangeDispatcher _dispatcher;
    private readonly IConfiguration _configuration;

    public Worker(IMessageBus messageBus, IEntityChangeDispatcher dispatcher, IConfiguration configuration)
    {
        _messageBus = messageBus;
        _dispatcher = dispatcher;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("[*] Worker started, waiting for messages...");

        var exchangeName = _configuration["RabbitMq:ExchangeName"];
        var queueName = _configuration["RabbitMq:QueueName"];

        await _messageBus.SubscribeAsync<EntityChangedEvent<JsonElement>>(
            topic: exchangeName,
            handler: async (entityChangedEvent, cancellationToken) =>
            {
                try
                {
                    Console.WriteLine($"[x] Received event for entity {entityChangedEvent.EntityType}");

                    await _dispatcher.DispatchAsync(
                        entityChangedEvent.EntityType,
                        entityChangedEvent.Data.GetRawText(),
                        entityChangedEvent.ChangeType,
                        cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error processing event: {ex.Message}");
                    throw;
                }
            },
            cancellationToken: stoppingToken,
            queueName: queueName
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
