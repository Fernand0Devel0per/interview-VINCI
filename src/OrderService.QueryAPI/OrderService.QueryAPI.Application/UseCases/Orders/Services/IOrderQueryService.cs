using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Orders.Dtos;

namespace OrderService.QueryAPI.Application.UseCases.Orders.Services;

public interface IOrderQueryService
{
    Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<OrderResponseDto>>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
}