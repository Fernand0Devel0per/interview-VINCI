using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Products.Dtos;

namespace OrderService.QueryAPI.Application.UseCases.Products.Services;

public interface IProductQueryService
{
    Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsAsync(CancellationToken cancellationToken = default);
}