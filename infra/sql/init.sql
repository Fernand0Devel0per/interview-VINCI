-- Criação do banco de dados
CREATE DATABASE CommandDb;
GO

USE CommandDb;
GO

CREATE TABLE Customers (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Orders (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    CustomerId UNIQUEIDENTIFIER NOT NULL,
    OrderDate DATETIME2 NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

CREATE TABLE Products (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    OrderId UNIQUEIDENTIFIER NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);
GO

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
        Console.WriteLine("[*] Worker started, listening for messages...");

        var exchangeName = _configuration["RabbitMq:ExchangeName"];
        var queueName = _configuration["RabbitMq:QueueName"];

        await _messageBus.SubscribeAsync<EntityChangedEvent<JsonElement>>(
            topic: exchangeName,
            handler: async (entityChangedEvent, cancellationToken) =>
            {
                try
                {
                    await _dispatcher.DispatchAsync(
                        entityChangedEvent.EntityType,
                        entityChangedEvent.Data.GetRawText(),
                        entityChangedEvent.ChangeType,
                        cancellationToken
                    );

                    Console.WriteLine($"[x] Processed event of type {entityChangedEvent.EntityType} with change {entityChangedEvent.ChangeType}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Error processing message: {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            },
            cancellationToken: stoppingToken,
            queueName: queueName
        );
        await _channel.BasicConsumeAsync(queue: "entity-changes-queue", autoAck: false, consumer: consumer);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
