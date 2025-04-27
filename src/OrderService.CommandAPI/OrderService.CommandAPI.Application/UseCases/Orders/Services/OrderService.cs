using BuildingBlocks.Core.ApiResponses;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Orders.DTOs;
using OrderService.CommandAPI.Application.UseCases.Orders.Mappings;
using OrderService.CommandAPI.Domain.Repositories;
using OrderService.CommandAPI.Infrastructure.Data;

namespace OrderService.CommandAPI.Application.UseCases.Orders.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
        private readonly CommandDbContext _dbContext;
        private readonly EventPublisherService _eventPublisherService;
    
        public OrderService(IOrderRepository orderRepository, CommandDbContext dbContext, EventPublisherService eventPublisherService)
        {
            _orderRepository = orderRepository;
            _dbContext = dbContext;
            _eventPublisherService = eventPublisherService;
        }

    public async Task<IApiResponse> CreateOrderAsync(CreateOrderDto requestDto, CancellationToken cancellationToken = default)
    {
        var order = requestDto.ToEntity();

        await _orderRepository.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Created,
            order,
            cancellationToken: cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(order.Id, "Order created successfully.");
    }

    public async Task<IApiResponse> DeleteOrderAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        if (order is null)
            return ApiResponse<string>.Fail(new List<string> { "Order not found" }, "Not Found");
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Deleted,
            order,
            cancellationToken: cancellationToken
        );
        
        await _orderRepository.DeleteAsync(id, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(id, "Order deleted successfully.");
    }
}