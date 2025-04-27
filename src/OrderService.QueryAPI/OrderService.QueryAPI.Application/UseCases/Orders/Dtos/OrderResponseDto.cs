namespace OrderService.QueryAPI.Application.UseCases.Orders.Dtos;

public class OrderResponseDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public List<ProductInOrderResponseDto> Products { get; set; } = new();
}