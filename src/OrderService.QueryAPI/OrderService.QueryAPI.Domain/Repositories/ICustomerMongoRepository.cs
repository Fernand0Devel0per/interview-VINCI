using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Domain.Repositories;

public interface ICustomerMongoRepository : IMongoRepository<Customer>
{
}