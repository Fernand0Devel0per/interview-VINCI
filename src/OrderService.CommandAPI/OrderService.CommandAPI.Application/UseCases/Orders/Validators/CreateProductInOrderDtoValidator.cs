using FluentValidation;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Orders.Validators;

public class CreateProductInOrderDtoValidator : AbstractValidator<CreateProductInOrderDto>
{
    public CreateProductInOrderDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .NotEqual(Guid.Empty).WithMessage("ProductId must be a valid GUID.");
    }
}