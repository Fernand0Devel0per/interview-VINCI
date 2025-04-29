using MongoDB.Driver;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories.Persistence.Mongo;

public class OrderMongoRepository : MongoRepository<Order>, IOrderMongoRepository
{
    public OrderMongoRepository(IMongoDatabase database) : base(database, "Order") { }
}