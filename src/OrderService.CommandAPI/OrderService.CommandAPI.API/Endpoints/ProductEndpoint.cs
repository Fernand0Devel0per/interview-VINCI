using Microsoft.AspNetCore.Mvc;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Application.UseCases.Products.Services;

namespace OrderService.CommandAPI.API.Endpoints;

public static class ProductEndpoint
{
    public static void ConfigureProductEndpoints(this WebApplication app)
    {
        app.MapPost("/api/products", CreateProduct)
            .WithName("CreateProduct")
            .Accepts<CreateProductDto>("application/json")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("/api/products/{id}", UpdateProduct)
            .WithName("UpdateProduct")
            .Accepts<UpdateProductDto>("application/json")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("/api/products/{id}", DeleteProduct)
            .WithName("DeleteProduct")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    private static async Task<IResult> CreateProduct(IProductService productService, [FromBody] CreateProductDto createDto)
    {
        var response = await productService.CreateProductAsync(createDto);
        return Results.Created($"/api/products/{((dynamic)response).Data}", response);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    private static async Task<IResult> UpdateProduct(IProductService productService, [FromRoute] Guid id, [FromBody] UpdateProductDto updateDto)
    {
        var response = await productService.UpdateProductAsync(id, updateDto);
        return Results.Ok(response);
    }

    /// <summary>
    /// Deletes a product by ID.
    /// </summary>
    private static async Task<IResult> DeleteProduct(IProductService productService, [FromRoute] Guid id)
    {
        var response = await productService.DeleteProductAsync(id);
        return Results.Ok(response);
    }
}
