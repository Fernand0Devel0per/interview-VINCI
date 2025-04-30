using BuildingBlocks.Caching.Abstractions;
using NSubstitute;
using OrderService.QueryAPI.Application.UseCases.Products.Dtos;
using OrderService.QueryAPI.Application.UseCases.Products.Services;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Products;

public class ProductQueryServiceTests
{
    private readonly IProductMongoRepository _productRepository = Substitute.For<IProductMongoRepository>();
    private readonly ICacheService _cacheService = Substitute.For<ICacheService>();
    private readonly ProductQueryService _service;

    public ProductQueryServiceTests()
    {
        _service = new ProductQueryService(_productRepository, _cacheService);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductIsCached()
    {
        var productId = Guid.NewGuid();
        var cachedDto = new ProductResponseDto { Id = productId, Name = "Cached Product", Price = 20.0m };
        _cacheService.GetAsync<ProductResponseDto>($"product:{productId}", Arg.Any<CancellationToken>())
            .Returns(cachedDto);
        
        var response = await _service.GetProductByIdAsync(productId);
        
        Assert.True(response.IsSuccess);
        Assert.Equal(productId, ((ProductResponseDto)response.Data!).Id);
        await _productRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProductFromRepository_WhenNotInCache()
    {
        var productId = Guid.NewGuid();
        var product = new Product(productId, "Test Product", 30.5m);

        _cacheService.GetAsync<ProductResponseDto>($"product:{productId}", Arg.Any<CancellationToken>())
            .Returns((ProductResponseDto?)null);

        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns(product);
        
        var response = await _service.GetProductByIdAsync(productId);
        
        Assert.True(response.IsSuccess);
        Assert.Equal(productId, ((ProductResponseDto)response.Data!).Id);
        await _cacheService.Received(1).SetAsync(
            $"product:{productId}",
            Arg.Any<ProductResponseDto>(),
            Arg.Any<TimeSpan?>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnFail_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        _cacheService.GetAsync<ProductResponseDto>($"product:{productId}", Arg.Any<CancellationToken>())
            .Returns((ProductResponseDto?)null);
        _productRepository.GetByIdAsync(productId, Arg.Any<CancellationToken>())
            .Returns((Product?)null);
        
        var response = await _service.GetProductByIdAsync(productId);
        
        Assert.False(response.IsSuccess);
        Assert.Equal("Not Found", response.Message);
    }
}