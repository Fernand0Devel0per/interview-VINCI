using Microsoft.AspNetCore.Mvc;
using OrderService.QueryAPI.Application.UseCases.Products.Services;
using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Products.Dtos;

namespace OrderService.QueryAPI.API.Endpoints;

public static class ProductEndpoint
{
    public static void ConfigureProductEndpoints(this WebApplication app)
    {
        app.MapGet("/api/products/{id}", GetProductById)
            .WithName("GetProductById")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("/api/products", GetAllProducts)
            .WithName("GetAllProducts")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Retrieves a product by ID.
    /// </summary>
    private static async Task<IResult> GetProductById(IProductQueryService productQueryService, [FromRoute] Guid id)
    {
        var product = (ApiResponse<ProductResponseDto>) await productQueryService.GetProductByIdAsync(id);

        if (product.Data is null)
            return Results.NotFound();

        return Results.Ok(product);
    }

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    private static async Task<IResult> GetAllProducts(IProductQueryService productQueryService)
    {
        var products = (ApiResponse<List<ProductResponseDto>>) await productQueryService.GetAllProductsAsync();

        if (products.Data is null || !products.Data.Any())
            return Results.NoContent();

        return Results.Ok(products);
    }
}