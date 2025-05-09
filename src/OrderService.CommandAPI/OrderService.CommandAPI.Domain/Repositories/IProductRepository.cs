using OrderService.CommandAPI.Domain.Entities;

namespace OrderService.CommandAPI.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);
}