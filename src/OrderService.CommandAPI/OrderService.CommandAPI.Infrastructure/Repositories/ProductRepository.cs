using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.Domain.Entities;
using OrderService.CommandAPI.Domain.Repositories;
using OrderService.CommandAPI.Infrastructure.Data;

namespace OrderService.CommandAPI.Infrastructure.Repositories;

public class ProductRepository : SqlRepository<Product>, IProductRepository
{
    public ProductRepository(CommandDbContext dbContext) : base(dbContext) { }
    
    public async Task<List<Product>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(cancellationToken);
    }
}