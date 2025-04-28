using Microsoft.AspNetCore.Mvc;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Application.UseCases.Customers.Services;

namespace OrderService.CommandAPI.API.Endpoints;

public static class CustomerEndpoint
{
    public static void ConfigureCustomerEndpoints(this WebApplication app)
    {
        app.MapPost("/api/customers", CreateCustomer)
            .WithValidator<CreateCustomerDto>()
            .WithName("CreateCustomer")
            .Accepts<CreateCustomerDto>("application/json")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("/api/customers/{id}", UpdateCustomer)
            .WithValidator<UpdateCustomerDto>()
            .WithName("UpdateCustomer")
            .Accepts<UpdateCustomerDto>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/customers/{id}", DeleteCustomer)
            .WithName("DeleteCustomer")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="createDto">Customer details to create.</param>
    /// <returns>The result of the creation.</returns>
    private async static Task<IResult> CreateCustomer(ICustomerService customerService, [FromBody] CreateCustomerDto createDto)
    {
        try
        {
            var response = await customerService.CreateCustomerAsync(createDto);
            return Results.Created($"/api/customers/{((dynamic)response).Data}", response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Error: {ex.Message}");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="id">The customer ID.</param>
    /// <param name="updateDto">Customer updated details.</param>
    /// <returns>The result of the update.</returns>
    private async static Task<IResult> UpdateCustomer(ICustomerService customerService, [FromRoute] Guid id, [FromBody] UpdateCustomerDto updateDto)
    {
        try
        {
            var response = await customerService.UpdateCustomerAsync(id, updateDto);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Error: {ex.Message}");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Deletes a customer by ID.
    /// </summary>
    /// <param name="customerService">The customer service.</param>
    /// <param name="id">The customer ID to delete.</param>
    /// <returns>The result of the deletion.</returns>
    private async static Task<IResult> DeleteCustomer(ICustomerService customerService, [FromRoute] Guid id)
    {
        try
        {
            var response = await customerService.DeleteCustomerAsync(id);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Error: {ex.Message}");
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
