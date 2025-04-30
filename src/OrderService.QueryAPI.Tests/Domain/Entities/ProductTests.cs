using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Tests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        var id = Guid.NewGuid();
        var name = "Product A";
        var price = 99.99m;
        
        var product = new Product(id, name, price);
        
        Assert.Equal(id, product.Id);
        Assert.Equal(name, product.Name);
        Assert.Equal(price, product.Price);
    }
}