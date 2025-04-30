using BuildingBlocks.Caching.Abstractions;
using FluentAssertions;
using NSubstitute;
using OrderService.QueryAPI.Application.UseCases.Orders.Dtos;
using OrderService.QueryAPI.Application.UseCases.Orders.Services;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Orders
{
    public class OrderQueryServiceTests
    {
        private readonly IOrderMongoRepository _repository = Substitute.For<IOrderMongoRepository>();
        private readonly ICacheService _cacheService = Substitute.For<ICacheService>();
        private readonly OrderQueryService _service;

        public OrderQueryServiceTests()
        {
            _service = new OrderQueryService(_repository, _cacheService);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnCachedOrder_WhenExists()
        {
            var orderId = Guid.NewGuid();
            var cached = new OrderResponseDto
            {
                Id = orderId,
                CustomerId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                TotalAmount = 50,
                Products = new()
            };

            _cacheService.GetAsync<OrderResponseDto>($"order:{orderId}", Arg.Any<CancellationToken>())
                .Returns(cached);

            var result = await _service.GetOrderByIdAsync(orderId);

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().BeEquivalentTo(cached);
            await _repository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            var id = Guid.NewGuid();

            _cacheService.GetAsync<OrderResponseDto>($"order:{id}", Arg.Any<CancellationToken>())
                .Returns((OrderResponseDto?)null);

            _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
                .Returns((Order?)null);

            var result = await _service.GetOrderByIdAsync(id);

            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Order not found");
        }

        [Fact]
        public async Task GetAllOrdersAsync_ShouldReturnListOfOrders()
        {
            var orders = new List<Order>
            {
                new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 100),
                new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 200)
            };

            _repository.GetAllAsync(Arg.Any<CancellationToken>())
                .Returns(orders);

            var result = await _service.GetAllOrdersAsync();

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().HaveCount(2);
        }
    }
}
