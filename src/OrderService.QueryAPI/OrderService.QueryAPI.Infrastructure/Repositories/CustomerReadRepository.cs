using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories;

public class CustomerReadRepository : MongoRepository<Customer>, ICustomerReadRepository
{
    public CustomerReadRepository(IMongoDatabase database) : base(database, "Customers") { }
}