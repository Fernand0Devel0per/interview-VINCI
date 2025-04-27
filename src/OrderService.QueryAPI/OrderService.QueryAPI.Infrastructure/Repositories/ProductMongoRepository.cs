using MongoDB.Driver;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories;

public class ProductMongoRepository : MongoRepository<Product>, IProductMongoRepository
{
    public ProductMongoRepository(IMongoDatabase database) : base(database, "Customers") { }
}