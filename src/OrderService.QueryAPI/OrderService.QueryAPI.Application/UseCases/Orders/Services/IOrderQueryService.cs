using BuildingBlocks.Core.ApiResponses;

namespace OrderService.QueryAPI.Application.UseCases.Orders.Services;

public interface IOrderQueryService
{
    Task<IApiResponse> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IApiResponse> GetAllOrdersAsync(CancellationToken cancellationToken = default);
}