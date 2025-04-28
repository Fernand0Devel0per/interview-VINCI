using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public static class ValidationFilter
{
    public static RouteHandlerBuilder WithValidator<T>(this RouteHandlerBuilder builder) where T : class
    {
        builder.AddEndpointFilter(async (context, next) =>
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            var entity = context.Arguments.OfType<T>().FirstOrDefault();

            if (validator is not null && entity is not null)
            {
                ValidationResult validationResult = await validator.ValidateAsync(entity);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    return Results.ValidationProblem(errors);
                }
            }

            return await next(context);
        });

        return builder;
    }
}
