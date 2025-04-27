using MongoDB.Driver;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories;

public class CustomerMongoRepository : MongoRepository<Customer>, ICustomerMongoRepository
{
    public CustomerMongoRepository(IMongoDatabase database) : base(database, "Customers") { }
}