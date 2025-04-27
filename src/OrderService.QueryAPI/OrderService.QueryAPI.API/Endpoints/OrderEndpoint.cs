using Microsoft.AspNetCore.Mvc;
using OrderService.QueryAPI.Application.UseCases.Orders.Services;
using OrderService.QueryAPI.Application.UseCases.Orders.DTOs;
using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Orders.Dtos;

namespace OrderService.QueryAPI.API.Endpoints;

public static class OrderEndpoint
{
    public static void ConfigureOrderEndpoints(this WebApplication app)
    {
        app.MapGet("/api/orders/{id}", GetOrderById)
            .WithName("GetOrderById")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/orders", GetAllOrders)
            .WithName("GetAllOrders")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Retrieves an order by ID.
    /// </summary>
    private static async Task<IResult> GetOrderById(IOrderQueryService orderQueryService, [FromRoute] Guid id)
    {
        var order = (ApiResponse<OrderResponseDto>) await orderQueryService.GetOrderByIdAsync(id);

        if (order.Data is null)
            return Results.NotFound();

        return Results.Ok(order);
    }

    /// <summary>
    /// Retrieves all orders.
    /// </summary>
    private static async Task<IResult> GetAllOrders(IOrderQueryService orderQueryService)
    {
        var orders = (ApiResponse<List<OrderResponseDto>>) await orderQueryService.GetAllOrdersAsync();

        if (orders.Data is null || !orders.Data.Any())
            return Results.NoContent();

        return Results.Ok(orders);
    }
}