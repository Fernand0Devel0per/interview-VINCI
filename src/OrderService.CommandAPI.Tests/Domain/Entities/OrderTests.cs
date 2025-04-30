using FluentAssertions;
using OrderService.CommandAPI.Domain.Entities;

namespace OrderService.CommandAPI.Tests.Domain.Entities;

public class OrderTests
{
    [Fact]
    public void CreateOrder_ShouldInitializeCorrectly()
    {
        var customerId = Guid.NewGuid();
        
        var order = new Order(customerId);
        
        order.CustomerId.Should().Be(customerId);
        order.Products.Should().BeEmpty();
        order.TotalAmount.Should().Be(0);
        order.OrderDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }
    
    [Fact]
    public void AddMultipleProducts_ShouldAccumulateCorrectly()
    {
        var order = new Order(Guid.NewGuid());
        order.AddProduct(new Product("P1", 10));
        order.AddProduct(new Product("P2", 15));

        order.Products.Should().HaveCount(2);
        order.TotalAmount.Should().Be(25.00m);
    }
    
    [Fact]
    public void AddProductOrder_Should_Add_OrderProduct_To_Collection()
    {
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var order = new Order(customerId);
        
        order.AddProductOrder(productId);
        
        order.OrderProducts.Should().ContainSingle(op =>
            op.ProductId == productId && op.OrderId == order.Id);
    }
}