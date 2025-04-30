using FluentAssertions;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Application.UseCases.Products.Mappings;
using OrderService.CommandAPI.Domain.Entities;
using Xunit;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Products;

public class ProductMappingsTests
{
    [Fact]
    public void ToEntity_Should_Map_Correctly()
    {
        var dto = new CreateProductDto { Name = "Product A", Price = 99.99m };

        var entity = dto.ToEntity();

        entity.Name.Should().Be(dto.Name);
        entity.Price.Should().Be(dto.Price);
    }

    [Fact]
    public void UpdateEntity_Should_Update_Correctly()
    {
        var dto = new UpdateProductDto { Name = "Updated", Price = 123.45m };
        var entity = new Product("Original", 10m);

        dto.UpdateEntity(entity);

        entity.Name.Should().Be("Updated");
        entity.Price.Should().Be(123.45m);
    }

    [Fact]
    public void ToResponseDto_Should_Map_Correctly()
    {
        var entity = new Product("Product B", 50.5m);
        var dto = entity.ToResponseDto();

        dto.Id.Should().Be(entity.Id);
        dto.Name.Should().Be(entity.Name);
        dto.Price.Should().Be(entity.Price);
    }
}