using System.Text.Json;
using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.Enums;
using NSubstitute;
using OrderService.QueryAPI.Application.EntityChanges;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Products;

public class ProductChangeHandlerTests
{
    private readonly IProductMongoRepository _repository = Substitute.For<IProductMongoRepository>();
    private readonly ICacheService _cache = Substitute.For<ICacheService>();
    private readonly ProductChangeHandler _handler;

    public ProductChangeHandlerTests()
    {
        _handler = new ProductChangeHandler(_repository, _cache);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpsert_WhenCreated()
    {
        var product = new Product(Guid.NewGuid(), "Test Product", 99.99m);
        var json = JsonSerializer.Serialize(product);

        await _handler.HandleAsync(json, EntityChangeType.Created, default);

        await _repository.Received(1).UpsertAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync($"product:{product.Id}", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldDelete_WhenDeleted()
    {
        var product = new Product(Guid.NewGuid(), "Test Product", 99.99m);
        var json = JsonSerializer.Serialize(product);

        await _handler.HandleAsync(json, EntityChangeType.Deleted, default);

        await _repository.Received(1).DeleteAsync(product.Id, Arg.Any<CancellationToken>());
        await _cache.Received(1).RemoveAsync($"product:{product.Id}", Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldThrow_WhenDeserializationFails()
    {
        var invalidJson = "{ invalid json }";

        await Assert.ThrowsAsync<JsonException>(() =>
            _handler.HandleAsync(invalidJson, EntityChangeType.Created, default));
    }
}