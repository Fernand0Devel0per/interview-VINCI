using BuildingBlocks.Core.Interfaces;

namespace OrderService.CommandAPI.Domain.Entities;

public class Order : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public decimal TotalAmount { get; private set; }

    public List<Product> Products { get; private set; } = new();

    public Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        OrderDate = DateTime.UtcNow;
        TotalAmount = 0;
    }

    private Order() { }

    public void AddProduct(Product product)
    {
        Products.Add(product);
        TotalAmount += product.Price;
    }
}