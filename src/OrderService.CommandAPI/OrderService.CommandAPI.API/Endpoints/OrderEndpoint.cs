using Microsoft.AspNetCore.Mvc;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OrderService.CommandAPI.Application.UseCases.Orders.Services;

namespace OrderService.CommandAPI.API.Endpoints;

public static class OrderEndpoint
{
    public static void ConfigureOrderEndpoints(this WebApplication app)
    {
        app.MapPost("/api/orders", CreateOrder)
            .WithName("CreateOrder")
            .Accepts<CreateOrderDto>("application/json")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/orders/{id}", DeleteOrder)
            .WithName("DeleteOrder")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    private static async Task<IResult> CreateOrder(IOrderService orderService, [FromBody] CreateOrderDto createDto)
    {
        var response = await orderService.CreateOrderAsync(createDto);
        return Results.Created($"/api/orders/{((dynamic)response).Data}", response);
    }

    /// <summary>
    /// Deletes an order by ID.
    /// </summary>
    private static async Task<IResult> DeleteOrder(IOrderService orderService, [FromRoute] Guid id)
    {
        var response = await orderService.DeleteOrderAsync(id);
        return Results.Ok(response);
    }
}