using FluentValidation.TestHelper;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Application.UseCases.Products.Validators;
using Xunit;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Products;

public class CreateProductDtoValidatorTests
{
    private readonly CreateProductDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new CreateProductDto { Name = "", Price = 10 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var dto = new CreateProductDto { Name = new string('a', 101), Price = 10 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Price_Is_Zero()
    {
        var dto = new CreateProductDto { Name = "Valid", Price = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CreateProductDto { Name = "Product", Price = 10 };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}