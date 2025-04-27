namespace OrderService.CommandAPI.Application.UseCases.Orders.DTOs;

public class ProductInOrderResponseDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}