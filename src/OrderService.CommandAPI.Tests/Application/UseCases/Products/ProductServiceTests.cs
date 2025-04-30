using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Application.UseCases.Products.Services;
using OrderService.CommandAPI.Domain.Entities;
using OrderService.CommandAPI.Domain.Repositories;
using Xunit;
using BuildingBlocks.Core.ApiResponses;
using BuildingBlocks.Core.Enums;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Products;

public class ProductServiceTests
{
    private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IEventPublisherService _eventPublisher = Substitute.For<IEventPublisherService>();
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _service = new ProductService(_repository, _unitOfWork, _eventPublisher);
    }

    [Fact]
    public async Task CreateProductAsync_Should_Create_And_Publish_Event()
    {
        var dto = new CreateProductDto { Name = "Mouse", Price = 150 };
        
        var result = await _service.CreateProductAsync(dto);
        
        result.Should().BeOfType<ApiResponse<Guid>>();
        await _repository.Received(1).AddAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishEntityChangedEventAsync(
            EntityChangeType.Created,
            Arg.Any<Product>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>()
        );
    }

    [Fact]
    public async Task UpdateProductAsync_Should_Return_NotFound_When_Product_Not_Exists()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Product)null);
        
        var result = await _service.UpdateProductAsync(id, new UpdateProductDto());
        
        result.Should().BeOfType<ApiResponse<string>>()
              .Which.Errors.Should().Contain("Product not found");
    }

    [Fact]
    public async Task DeleteProductAsync_Should_Return_NotFound_When_Product_Not_Exists()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Product)null);
        
        var result = await _service.DeleteProductAsync(id);
        
        result.Should().BeOfType<ApiResponse<string>>()
              .Which.Errors.Should().Contain("Product not found");
    }
}
