using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OrderService.CommandAPI.Domain.Entities;

namespace OrderService.CommandAPI.Application.UseCases.Orders.Mappings;

public static class OrderMappings
{
    public static Order ToEntity(this CreateOrderDto dto)
    {
        var order = new Order(dto.CustomerId);

        foreach (var productDto in dto.Products)
        {
            var product = new Product(productDto.Name, productDto.Price);
            order.AddProduct(product);
        }

        return order;
    }

    public static OrderResponseDto ToResponseDto(this Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Products = order.Products.Select(p => new ProductInOrderResponseDto
            {
                Name = p.Name,
                Price = p.Price
            }).ToList()
        };
    }
}