using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Domain.Entities;

namespace OrderService.CommandAPI.Application.UseCases.Products.Mappings;

public static class ProductMappings
{
    public static Product ToEntity(this CreateProductDto dto)
    {
        return new Product(dto.Name, dto.Price);
    }

   public static void UpdateEntity(this UpdateProductDto dto, Product product)
   {
       product.UpdateName(dto.Name);
       product.UpdatePrice(dto.Price);
   }

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