using BuildingBlocks.Core.Interfaces;

namespace OrderService.CommandAPI.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : IEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}