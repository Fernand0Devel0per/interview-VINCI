using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories;

public class OrderReadRepository : MongoRepository<Order>, IOrderReadRepository
{
    public OrderReadRepository(IMongoDatabase database) : base(database, "Customers") { }
}