namespace OrderService.QueryAPI.Application.UseCases.Customers.DTOs;

public class CustomerResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}