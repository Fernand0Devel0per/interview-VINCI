using OrderService.QueryAPI.Application.UseCases.Customers.DTOs;
using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Application.UseCases.Customers.Mappings;

public static class CustomerMappings
{
    public static CustomerResponseDto ToResponseDto(this Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email
        };
    }
}