namespace OrderService.CommandAPI.Application.UseCases.Products.DTOs;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}