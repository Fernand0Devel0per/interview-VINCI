using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Orders.Dtos;
using OrderService.QueryAPI.Application.UseCases.Orders.Mappings;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.UseCases.Orders.Services;

public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderMongoRepository _orderRepository;

    public OrderQueryService(IOrderMongoRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<IApiResponse> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return ApiResponse<string>.Fail(new List<string> { "Order not found" }, "Not Found");

        return ApiResponse<OrderResponseDto>.Ok(order.ToResponseDto(), "Order retrieved successfully.");
    }

    public async Task<IApiResponse> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);

        var result = orders.Select(x => x.ToResponseDto()).ToList();

        return ApiResponse<List<OrderResponseDto>>.Ok(result, "Orders retrieved successfully.");
    }
}