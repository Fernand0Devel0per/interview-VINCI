using FluentAssertions;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Application.UseCases.Customers.Mappings;
using OrderService.CommandAPI.Domain.Entities;
using Xunit;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Customers;

public class CustomerMappingsTests
{
    [Fact]
    public void ToEntity_Should_Map_Correctly()
    {
        var dto = new CreateCustomerDto { Name = "John", Email = "john@example.com" };

        var entity = dto.ToEntity();

        entity.Name.Should().Be(dto.Name);
        entity.Email.Should().Be(dto.Email);
    }

    [Fact]
    public void UpdateEntity_Should_Update_Correctly()
    {
        var dto = new UpdateCustomerDto { Name = "Updated", Email = "new@example.com" };
        var entity = new Customer("Original", "original@example.com");

        dto.UpdateEntity(entity);

        entity.Name.Should().Be("Updated");
        entity.Email.Should().Be("new@example.com");
    }

    [Fact]
    public void ToResponseDto_Should_Map_Correctly()
    {
        var entity = new Customer("Jane", "jane@example.com");
        var dto = entity.ToResponseDto();

        dto.Id.Should().Be(entity.Id);
        dto.Name.Should().Be(entity.Name);
        dto.Email.Should().Be(entity.Email);
    }
}