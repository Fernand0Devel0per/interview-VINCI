using FluentAssertions;
using OrderService.QueryAPI.Application.UseCases.Customers.Mappings;
using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Customers
{
    public class CustomerMappingsTests
    {
        [Fact]
        public void ToResponseDto_ShouldMapCorrectly()
        {
            var customer = new Customer(Guid.NewGuid(), "John Doe", "john@example.com");

            var dto = customer.ToResponseDto();

            dto.Id.Should().Be(customer.Id);
            dto.Name.Should().Be(customer.Name);
            dto.Email.Should().Be(customer.Email);
        }
    }
}