namespace OrderService.QueryAPI.Application.UseCases.Orders.Dtos;

public class ProductInOrderResponseDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}