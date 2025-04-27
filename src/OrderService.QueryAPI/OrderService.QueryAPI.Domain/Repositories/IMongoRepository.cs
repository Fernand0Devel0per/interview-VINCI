using BuildingBlocks.Core.Interfaces;

namespace OrderService.QueryAPI.Domain.Repositories;

public interface IMongoRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, TEntity entity, CancellationToken cancellationToken = default);
}