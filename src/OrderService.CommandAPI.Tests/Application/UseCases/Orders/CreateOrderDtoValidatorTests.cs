using FluentValidation.TestHelper;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OrderService.CommandAPI.Application.UseCases.Orders.Validators;

public class CreateOrderDtoValidatorTests
{
    private readonly CreateOrderDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_CustomerId_Is_Empty()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.Empty,
            Products = new List<CreateProductInOrderDto> { new() { ProductId = Guid.NewGuid() } }
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.CustomerId);
    }

    [Fact]
    public void Should_Have_Error_When_Products_Is_Null()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = null!
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Products);
    }

    [Fact]
    public void Should_Have_Error_When_Products_Is_Empty()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateProductInOrderDto>()
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Products);
    }

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateProductInOrderDto> { new() { ProductId = Guid.Empty } }
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor("Products[0].ProductId");
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CreateOrderDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<CreateProductInOrderDto> { new() { ProductId = Guid.NewGuid() } }
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}