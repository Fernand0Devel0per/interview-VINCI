using FluentValidation.TestHelper;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OrderService.CommandAPI.Application.UseCases.Orders.Validators;
using Xunit;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Orders;

public class CreateProductInOrderDtoValidatorTests
{
    private readonly CreateProductInOrderDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_ProductId_Is_Empty()
    {
        var dto = new CreateProductInOrderDto { ProductId = Guid.Empty };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.ProductId);
    }

    [Fact]
    public void Should_Pass_When_ProductId_Is_Valid()
    {
        var dto = new CreateProductInOrderDto { ProductId = Guid.NewGuid() };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}