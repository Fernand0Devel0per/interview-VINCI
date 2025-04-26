using BuildingBlocks.Core.Interfaces;

namespace OrderService.QueryAPI.Domain.Repositories;

public interface IReadRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}