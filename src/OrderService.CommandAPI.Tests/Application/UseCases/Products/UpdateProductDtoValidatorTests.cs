using FluentValidation.TestHelper;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Application.UseCases.Products.Validators;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Products;

public class UpdateProductDtoValidatorTests
{
    private readonly UpdateProductDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new UpdateProductDto { Name = "", Price = 10 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Negative()
    {
        var dto = new UpdateProductDto { Name = "Valid", Price = -5 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new UpdateProductDto { Name = "Updated Product", Price = 20 };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
