using BuildingBlocks.Core.Interfaces;

namespace OrderService.CommandAPI.Domain.Entities;

public class Order : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public DateTime OrderDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    
    public List<Product> Products { get; private set; } = new();
    
    private List<OrderProduct> _orderProducts = new();
    public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts.AsReadOnly();

    private Order() { }

    public Order(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        OrderDate = DateTime.UtcNow;
        TotalAmount = 0;
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
        TotalAmount += product.Price;
    }

    public void AddProductOrder(Guid productId)
    {
        var orderProduct = new OrderProduct(Id, productId);
        _orderProducts.Add(orderProduct);
    }
    
}
