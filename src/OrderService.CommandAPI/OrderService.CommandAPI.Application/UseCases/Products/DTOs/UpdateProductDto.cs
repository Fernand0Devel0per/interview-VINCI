namespace OrderService.CommandAPI.Application.UseCases.Products.DTOs;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}