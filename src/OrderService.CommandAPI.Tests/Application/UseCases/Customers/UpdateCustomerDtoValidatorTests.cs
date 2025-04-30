using FluentValidation.TestHelper;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Application.UseCases.Customers.Validators;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Customers;

public class UpdateCustomerDtoValidatorTests
{
    private readonly UpdateCustomerDtoValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_Name_Is_Empty(string? name)
    {
        var dto = new UpdateCustomerDto { Name = name!, Email = "test@example.com" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var dto = new UpdateCustomerDto
        {
            Name = new string('x', 101),
            Email = "test@example.com"
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    public void Should_Have_Error_When_Email_Is_Invalid(string email)
    {
        var dto = new UpdateCustomerDto { Name = "Test", Email = email };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new UpdateCustomerDto { Name = "Valid", Email = "valid@example.com" };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
