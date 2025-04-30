using FluentAssertions;
using OrderService.QueryAPI.Application.UseCases.Orders.Mappings;
using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Orders
{
    public class OrderMappingsTests
    {
        [Fact]
        public void ToResponseDto_ShouldMapCorrectly()
        {
            var order = new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 0);
            order.AddProduct(new Product(Guid.NewGuid(), "Mouse", 50.0m));
            order.AddProduct(new Product(Guid.NewGuid(), "Monitor", 500.0m));

            var dto = order.ToResponseDto();

            dto.Id.Should().Be(order.Id);
            dto.CustomerId.Should().Be(order.CustomerId);
            dto.OrderDate.Should().Be(order.OrderDate);
            dto.TotalAmount.Should().Be(order.TotalAmount);
            dto.Products.Should().HaveCount(2);
            dto.Products.Select(p => p.Name).Should().Contain(new[] { "Mouse", "Monitor" });
        }
    }
}