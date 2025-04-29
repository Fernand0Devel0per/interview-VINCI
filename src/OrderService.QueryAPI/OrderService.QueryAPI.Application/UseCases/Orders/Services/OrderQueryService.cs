using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Orders.Dtos;
using OrderService.QueryAPI.Application.UseCases.Orders.Mappings;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.UseCases.Orders.Services;

public class OrderQueryService : IOrderQueryService
{
    private readonly IOrderMongoRepository _orderRepository;
    private readonly ICacheService _cacheService;

    public OrderQueryService(IOrderMongoRepository orderRepository, ICacheService cacheService)
    {
        _orderRepository = orderRepository;
        _cacheService = cacheService;
    }

    public async Task<IApiResponse> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"order:{id}";

        var cachedOrder = await _cacheService.GetAsync<OrderResponseDto>(cacheKey, cancellationToken);
        if (cachedOrder is not null)
            return ApiResponse<OrderResponseDto>.Ok(cachedOrder);

        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return ApiResponse<string>.Fail(new List<string> { "Order not found" }, "Not Found");

        var responseDto = order.ToResponseDto();
        await _cacheService.SetAsync(cacheKey, responseDto, TimeSpan.FromMinutes(10), cancellationToken);

        return ApiResponse<OrderResponseDto>.Ok(responseDto, "Order retrieved successfully.");
    }

    public async Task<IApiResponse> GetAllOrdersAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);

        var result = orders.Select(x => x.ToResponseDto()).ToList();

        return ApiResponse<List<OrderResponseDto>>.Ok(result, "Orders retrieved successfully.");
    }
}