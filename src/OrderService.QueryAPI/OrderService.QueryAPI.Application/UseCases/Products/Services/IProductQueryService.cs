using BuildingBlocks.Core.ApiResponses;

namespace OrderService.QueryAPI.Application.UseCases.Products.Services;

public interface IProductQueryService
{
    Task<IApiResponse> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IApiResponse> GetAllProductsAsync(CancellationToken cancellationToken = default);
}