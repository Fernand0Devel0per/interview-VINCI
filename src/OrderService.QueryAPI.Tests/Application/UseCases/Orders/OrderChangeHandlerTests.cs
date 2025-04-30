using System.Text.Json;
using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.Enums;
using NSubstitute;
using OrderService.QueryAPI.Application.EntityChanges;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Orders;

public class OrderChangeHandlerTests
{
    private readonly IOrderMongoRepository _repository = Substitute.For<IOrderMongoRepository>();
    private readonly ICacheService _cache = Substitute.For<ICacheService>();
    private readonly OrderChangeHandler _handler;

    public OrderChangeHandlerTests()
    {
        _handler = new OrderChangeHandler(_repository, _cache);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpsert_WhenCreated()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 120.0m);
        var json = JsonSerializer.Serialize(order);

        await _handler.HandleAsync(json, EntityChangeType.Created, default);

        await _repository.Received(1).UpsertAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync($"order:{order.Id}", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldDelete_WhenDeleted()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 120.0m);
        var json = JsonSerializer.Serialize(order);

        await _handler.HandleAsync(json, EntityChangeType.Deleted, default);

        await _repository.Received(1).DeleteAsync(order.Id, Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync($"order:{order.Id}", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenDeserializationFails()
    {
        var invalidJson = "{ invalid json }";

        await Assert.ThrowsAsync<JsonException>(() =>
            _handler.HandleAsync(invalidJson, EntityChangeType.Created, default));
    }
}