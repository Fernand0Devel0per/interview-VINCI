using BuildingBlocks.Core.ApiResponses;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OrderService.CommandAPI.Application.UseCases.Orders.Mappings;
using OrderService.CommandAPI.Domain.Repositories;
using OrderService.CommandAPI.Infrastructure.Data;

namespace OrderService.CommandAPI.Application.UseCases.Orders.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly CommandDbContext _dbContext;

    public OrderService(IOrderRepository orderRepository, CommandDbContext dbContext)
    {
        _orderRepository = orderRepository;
        _dbContext = dbContext;
    }

    public async Task<IApiResponse> CreateOrderAsync(CreateOrderDto requestDto, CancellationToken cancellationToken = default)
    {
        var order = requestDto.ToEntity();

        await _orderRepository.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(order.Id, "Order created successfully.");
    }

    public async Task<IApiResponse> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return ApiResponse<string>.Fail(new List<string> { "Order not found" }, "Not Found");

        await _orderRepository.DeleteAsync(id, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(id, "Order deleted successfully.");
    }
}