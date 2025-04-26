using BuildingBlocks.Core.ApiResponses;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Products;

public interface IProductService
{
    Task<IApiResponse> CreateProductAsync(CreateProductDto requestDto, CancellationToken cancellationToken = default);
    Task<IApiResponse> UpdateProductAsync(Guid id, UpdateProductDto requestDto, CancellationToken cancellationToken = default);
    Task<IApiResponse> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
}