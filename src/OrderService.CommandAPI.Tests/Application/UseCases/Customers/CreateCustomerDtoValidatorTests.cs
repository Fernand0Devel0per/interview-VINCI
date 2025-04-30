using FluentValidation.TestHelper;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Application.UseCases.Customers.Validators;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Customers;

public class CreateCustomerDtoValidatorTests
{
    private readonly CreateCustomerDtoValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Have_Error_When_Name_Is_Empty(string? name)
    {
        var dto = new CreateCustomerDto { Name = name!, Email = "test@example.com" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var dto = new CreateCustomerDto
        {
            Name = new string('a', 101),
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
        var dto = new CreateCustomerDto { Name = "Test", Email = email };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new CreateCustomerDto { Name = "Valid Name", Email = "valid@example.com" };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}