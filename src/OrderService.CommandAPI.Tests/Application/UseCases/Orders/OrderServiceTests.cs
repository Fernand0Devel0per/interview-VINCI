using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OS = OrderService.CommandAPI.Application.UseCases.Orders.Services;
using OrderService.CommandAPI.Domain.Entities;
using OrderService.CommandAPI.Domain.Repositories;

using BuildingBlocks.Core.ApiResponses;
using BuildingBlocks.Core.Enums;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Orders;

public class OrderServiceTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IEventPublisherService _eventPublisher = Substitute.For<IEventPublisherService>();
    private readonly OS.OrderService _service;

    public OrderServiceTests()
    {
        _service = new OS.OrderService(_orderRepository, _productRepository, _unitOfWork, _eventPublisher, _customerRepository);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Fail_If_Customer_Not_Found()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateProductInOrderDto>()
        };

        _customerRepository.GetByIdAsync(dto.CustomerId, Arg.Any<CancellationToken>())
            .Returns((Customer)null);

        var result = await _service.CreateOrderAsync(dto);

        result.Should().BeOfType<ApiResponse<string>>()
              .Which.Errors.Should().Contain("Customer not found.");
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Fail_If_Any_Product_Not_Found()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateProductInOrderDto>
            {
                new() { ProductId = Guid.NewGuid() }
            }
        };

        _customerRepository.GetByIdAsync(dto.CustomerId, Arg.Any<CancellationToken>())
            .Returns(new Customer("Test", "email@test.com"));

        _productRepository.GetByIdsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<Product>()); // vazio

        var result = await _service.CreateOrderAsync(dto);

        result.Should().BeOfType<ApiResponse<string>>()
              .Which.Errors.Should().Contain("One or more products were not found.");
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Create_And_Publish_Event()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var dto = new CreateOrderDto
        {
            CustomerId = customerId,
            Products = new List<CreateProductInOrderDto>
            {
                new() { ProductId = productId }
            }
        };

        _customerRepository.GetByIdAsync(customerId, Arg.Any<CancellationToken>())
            .Returns(new Customer("Test", "email@test.com"));

        _productRepository.GetByIdsAsync(Arg.Any<List<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new List<Product>
            {
                new Product("Product A", 100)
            });

        var result = await _service.CreateOrderAsync(dto);

        result.Should().BeOfType<ApiResponse<Guid>>();
        await _orderRepository.Received(1).AddAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishEntityChangedEventAsync(
            EntityChangeType.Created,
            Arg.Any<Order>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteOrderAsync_Should_Fail_If_Not_Found()
    {
        var id = Guid.NewGuid();
        _orderRepository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Order)null);

        var result = await _service.DeleteOrderAsync(id);

        result.Should().BeOfType<ApiResponse<string>>()
              .Which.Errors.Should().Contain("Order not found");
    }

    [Fact]
    public async Task DeleteOrderAsync_Should_Delete_And_Publish_Event()
    {
        var id = Guid.NewGuid();
        _orderRepository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns(new Order(Guid.NewGuid()));

        var result = await _service.DeleteOrderAsync(id);

        result.Should().BeOfType<ApiResponse<Guid>>();
        await _orderRepository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishEntityChangedEventAsync(
            EntityChangeType.Deleted,
            Arg.Any<Order>(),
            Arg.Any<string>(),
            Arg.Any<CancellationToken>());
    }
}
