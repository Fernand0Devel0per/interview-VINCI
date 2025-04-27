using FluentValidation;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Orders.Validators;

public class CreateProductInOrderDtoValidator : AbstractValidator<CreateProductInOrderDto>
{
    public CreateProductInOrderDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}