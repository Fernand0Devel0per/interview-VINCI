using Microsoft.AspNetCore.Mvc;
using OrderService.QueryAPI.Application.UseCases.Customers.Services;
using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Customers.DTOs;

namespace OrderService.QueryAPI.API.Endpoints;

public static class CustomerEndpoint
{
    public static void ConfigureCustomerEndpoints(this WebApplication app)
    {
        app.MapGet("/api/customers/{id}", GetCustomerById)
            .WithName("GetCustomerById")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/customers", GetAllCustomers)
            .WithName("GetAllCustomers")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Retrieves a customer by ID.
    /// </summary>
    private static async Task<IResult> GetCustomerById(ICustomerQueryService customerQueryService, [FromRoute] Guid id)
    {
        var customer = (ApiResponse<CustomerResponseDto>) await customerQueryService.GetCustomerByIdAsync(id);

        if (customer.Data is null)
            return Results.NotFound();

        return Results.Ok(customer);
    }

    /// <summary>
    /// Retrieves all customers.
    /// </summary>
    private static async Task<IResult> GetAllCustomers(ICustomerQueryService customerQueryService)
    {
        var customers = (ApiResponse<List<CustomerResponseDto>>) await customerQueryService.GetAllCustomersAsync();

        if (customers.Data is null || !customers.Data.Any())
            return Results.NoContent();

        return Results.Ok(customers);
    }
}