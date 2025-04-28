using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.Domain.Entities;
using OrderService.CommandAPI.Domain.Repositories;
using OrderService.CommandAPI.Infrastructure.Data;

namespace OrderService.CommandAPI.Infrastructure.Repositories;

public class OrderRepository : SqlRepository<Order>, IOrderRepository
{
    public OrderRepository(CommandDbContext dbContext) : base(dbContext) { }
}