using OrderService.QueryAPI.Application.UseCases.Orders.Dtos;
using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Application.UseCases.Orders.Mappings;

public static class OrderMappings
{
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