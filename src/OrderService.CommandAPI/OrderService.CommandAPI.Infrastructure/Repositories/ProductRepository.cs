using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.Domain.Entities;
using OrderService.CommandAPI.Domain.Repositories;

namespace OrderService.CommandAPI.Infrastructure.Repositories;

public class ProductRepository : SqlRepository<Product>, IProductRepository
{
    public ProductRepository(DbContext dbContext) : base(dbContext) { }
}