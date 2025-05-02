using System.Diagnostics;
using System.Text.Json;
using BuildingBlocks.Core.Events;
using BuildingBlocks.Messaging.Abstractions;
using OrderService.QueryAPI.Application.Common.Abstractions;
using Serilog;

namespace OrderService.QueryAPI.Worker;

public class Worker : BackgroundService
{
    private static readonly ActivitySource ActivitySource = new("OrderService.Worker");

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
        Log.Information("[*] Worker started, waiting for messages...");

        var exchangeName = _configuration["RabbitMq:ExchangeName"];
        var queueName = _configuration["RabbitMq:QueueName"];

        await _messageBus.SubscribeAsync<EntityChangedEvent<JsonElement>>(
            topic: exchangeName,
            handler: async (entityChangedEvent, cancellationToken) =>
            {
                using var activity = ActivitySource.StartActivity("ProcessEntityChangedEvent", ActivityKind.Consumer);
                activity?.SetTag("messaging.system", "rabbitmq");
                activity?.SetTag("messaging.destination", exchangeName);
                activity?.SetTag("messaging.rabbitmq.queue", queueName);
                activity?.SetTag("entity.type", entityChangedEvent.EntityType);
                activity?.SetTag("entity.change_type", entityChangedEvent.ChangeType.ToString());

                using var scope = _serviceScopeFactory.CreateScope();
                var dispatcher = scope.ServiceProvider.GetRequiredService<IEntityChangeDispatcher>();

                try
                {
                    Log.Information("[x] Received event for entity {EntityType}", entityChangedEvent.EntityType);

                    await dispatcher.DispatchAsync(
                        entityChangedEvent.EntityType,
                        entityChangedEvent.Data.GetRawText(),
                        entityChangedEvent.ChangeType,
                        cancellationToken
                    );

                    activity?.SetStatus(ActivityStatusCode.Ok);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "[!] Error processing event for entity {EntityType}", entityChangedEvent.EntityType);
                    activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                    throw;
                }
            },
            cancellationToken: stoppingToken,
            queueName: queueName
        );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
