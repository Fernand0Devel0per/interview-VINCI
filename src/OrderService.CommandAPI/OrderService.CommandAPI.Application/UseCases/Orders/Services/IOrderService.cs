using BuildingBlocks.Core.ApiResponses;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Orders.Services;

public interface IOrderService
{
    Task<IApiResponse> CreateOrderAsync(CreateOrderDto requestDto, CancellationToken cancellationToken = default);
    Task<IApiResponse> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default);
}