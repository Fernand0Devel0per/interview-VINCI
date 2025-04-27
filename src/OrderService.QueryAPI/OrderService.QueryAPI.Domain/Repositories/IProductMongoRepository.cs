using OrderService.QueryAPI.Domain.Entities;

namespace OrderService.QueryAPI.Domain.Repositories;

public interface IProductMongoRepository : IMongoRepository<Product>
{
}