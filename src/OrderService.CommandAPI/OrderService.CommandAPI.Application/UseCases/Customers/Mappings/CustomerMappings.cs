using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Domain.Entities;

namespace OrderService.CommandAPI.Application.UseCases.Customers.Mappings;

public static class CustomerMappings
{
    public static Customer ToEntity(this CreateCustomerDto dto)
    {
        return new Customer(dto.Name, dto.Email);
    }

    public static void UpdateEntity(this UpdateCustomerDto dto, Customer customer)
    {
        customer.UpdateName(dto.Name);
        customer.UpdateEmail(dto.Email);
    }

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