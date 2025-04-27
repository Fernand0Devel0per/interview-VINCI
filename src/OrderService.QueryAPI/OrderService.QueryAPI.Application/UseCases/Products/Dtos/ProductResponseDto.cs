namespace OrderService.QueryAPI.Application.UseCases.Products.Dtos;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}