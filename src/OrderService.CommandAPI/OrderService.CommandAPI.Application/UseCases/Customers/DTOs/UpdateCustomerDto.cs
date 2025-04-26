namespace OrderService.CommandAPI.Application.UseCases.Customers.DTOs;

public class UpdateCustomerDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}