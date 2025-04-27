namespace OrderService.CommandAPI.Application.UseCases.Orders.DTOs;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }
    public List<CreateProductInOrderDto> Products { get; set; } = new();
}