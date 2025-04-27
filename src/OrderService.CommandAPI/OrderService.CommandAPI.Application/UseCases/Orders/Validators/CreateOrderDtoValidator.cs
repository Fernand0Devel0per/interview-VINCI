using FluentValidation;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Orders.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.Products)
            .NotNull().WithMessage("Products list must not be null.")
            .Must(products => products != null && products.Any())
                .WithMessage("Order must contain at least one product.");

        RuleForEach(x => x.Products).SetValidator(new CreateProductInOrderDtoValidator());
    }
}
