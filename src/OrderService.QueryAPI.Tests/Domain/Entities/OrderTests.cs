using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Tests.Domain.Entities;

public class OrderTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var orderDate = DateTime.UtcNow;
        var totalAmount = 0m;

        // Act
        var order = new Order(id, customerId, orderDate, totalAmount);

        // Assert
        Assert.Equal(id, order.Id);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(orderDate, order.OrderDate);
        Assert.Equal(totalAmount, order.TotalAmount);
        Assert.Empty(order.Products);
    }

    [Fact]
    public void AddProduct_ShouldAddToProductsAndUpdateTotal()
    {
        // Arrange
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, 0);
        var product = new Product(Guid.NewGuid(), "Test Product", 10.5m);

        // Act
        order.AddProduct(product);

        // Assert
        Assert.Contains(product, order.Products);
        Assert.Equal(10.5m, order.TotalAmount);
    }
}