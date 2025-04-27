using FluentValidation;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Customers.Validators;

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.");
    }
}