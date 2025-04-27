using MongoDB.Driver;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories.Persistence.Mongo;

public class CustomerMongoRepository : MongoRepository<Customer>, ICustomerMongoRepository
{
    public CustomerMongoRepository(IMongoDatabase database) : base(database, "Customers") { }
}