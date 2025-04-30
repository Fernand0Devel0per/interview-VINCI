using System.Text.Json;
using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.Enums;
using NSubstitute;
using OrderService.QueryAPI.Application.EntityChanges;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Customers;

public class CustomerChangeHandlerTests
{
    private readonly ICustomerMongoRepository _repository = Substitute.For<ICustomerMongoRepository>();
    private readonly ICacheService _cache = Substitute.For<ICacheService>();
    private readonly CustomerChangeHandler _handler;

    public CustomerChangeHandlerTests()
    {
        _handler = new CustomerChangeHandler(_repository, _cache);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpsert_WhenCreated()
    {
        var customer = new Customer(Guid.NewGuid(), "John", "john@example.com");
        var json = JsonSerializer.Serialize(customer);

        await _handler.HandleAsync(json, EntityChangeType.Created, default);

        await _repository.Received(1).UpsertAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync($"customer:{customer.Id}", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldDelete_WhenDeleted()
    {
        var customer = new Customer(Guid.NewGuid(), "John", "john@example.com");
        var json = JsonSerializer.Serialize(customer);

        await _handler.HandleAsync(json, EntityChangeType.Deleted, default);

        await _repository.Received(1).DeleteAsync(customer.Id, Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync($"customer:{customer.Id}", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenDeserializationFails()
    {
        var invalidJson = "{ invalid json }";

        await Assert.ThrowsAsync<JsonException>(() =>
            _handler.HandleAsync(invalidJson, EntityChangeType.Created, default));
    }
}