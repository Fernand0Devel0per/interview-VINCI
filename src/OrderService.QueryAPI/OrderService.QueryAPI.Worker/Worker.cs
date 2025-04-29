using System.Text.Json;
using BuildingBlocks.Core.Events;
using BuildingBlocks.Messaging.Abstractions;
using OrderService.QueryAPI.Application.Common.Abstractions;

public class Worker : BackgroundService
{
    private readonly IMessageBus _messageBus;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;

    public Worker(IMessageBus messageBus, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
    {
        _messageBus = messageBus;
        _serviceScopeFactory = serviceScopeFactory;
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
                using var scope = _serviceScopeFactory.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<IEntityChangeDispatcher>();

                try
                {
                    Console.WriteLine($"[x] Received event for entity {entityChangedEvent.EntityType}");

                    await dispatcher.DispatchAsync(
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