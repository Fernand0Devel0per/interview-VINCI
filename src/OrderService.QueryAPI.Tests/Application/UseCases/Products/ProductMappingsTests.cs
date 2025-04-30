using FluentAssertions;
using OrderService.QueryAPI.Application.UseCases.Products.Mappings;
using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Tests.Application.UseCases.Products
{
    public class ProductMappingsTests
    {
        [Fact]
        public void ToResponseDto_ShouldMapCorrectly()
        {
            var product = new Product(Guid.NewGuid(), "Keyboard", 99.99m);

            var dto = product.ToResponseDto();

            dto.Id.Should().Be(product.Id);
            dto.Name.Should().Be(product.Name);
            dto.Price.Should().Be(product.Price);
        }
    }
}