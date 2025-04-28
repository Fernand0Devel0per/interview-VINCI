using FluentValidation;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Customers.Validators;

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
    }
}