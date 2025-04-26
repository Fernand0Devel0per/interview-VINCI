using MongoDB.Driver;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories;

public class ProductReadRepository : MongoRepository<Product>, IProductReadRepository
{
    public ProductReadRepository(IMongoDatabase database) : base(database, "Customers") { }
}