using FluentAssertions;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OrderService.CommandAPI.Application.UseCases.Orders.Mappings;
using OrderService.CommandAPI.Domain.Entities;
using Xunit;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Orders;

public class OrderMappingsTests
{
    [Fact]
    public void ToEntity_Should_Map_Correctly()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateProductInOrderDto>
            {
                new CreateProductInOrderDto { ProductId = Guid.NewGuid() }
            }
        };

        var entity = dto.ToEntity();

        entity.CustomerId.Should().Be(dto.CustomerId);
        entity.OrderProducts.Should().ContainSingle();
    }

    [Fact]
    public void ToResponseDto_Should_Map_Correctly()
    {
        var order = new Order(Guid.NewGuid());
        order.AddProduct(new Product("P1", 10));
        order.AddProduct(new Product("P2", 20));

        var dto = order.ToResponseDto();

        dto.Id.Should().Be(order.Id);
        dto.CustomerId.Should().Be(order.CustomerId);
        dto.TotalAmount.Should().Be(30);
        dto.Products.Should().HaveCount(2);
    }
}