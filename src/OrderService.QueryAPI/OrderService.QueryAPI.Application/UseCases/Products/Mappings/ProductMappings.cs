
using OrderService.QueryAPI.Application.UseCases.Products.Dtos;
using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Application.UseCases.Products.Mappings;

public static class ProductMappings
{
    public static ProductResponseDto ToResponseDto(this Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}