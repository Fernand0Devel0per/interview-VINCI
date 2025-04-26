using OrderService.CommandAPI.Domain.Entities;
using OrderService.CommandAPI.Domain.Repositories;

namespace OrderService.CommandAPI.Infrastructure.Repositories;

public class OrderRepository : SqlRepository<Order>, IOrderRepository
{
    public OrderRepository(DbContext dbContext) : base(dbContext) { }
}