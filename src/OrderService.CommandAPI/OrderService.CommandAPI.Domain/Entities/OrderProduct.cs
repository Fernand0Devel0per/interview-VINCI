namespace OrderService.CommandAPI.Domain.Entities;

public class OrderProduct
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }

    private OrderProduct() { }

    public OrderProduct(Guid orderId, Guid productId)
    {
        OrderId = orderId;
        ProductId = productId;
    }
}
