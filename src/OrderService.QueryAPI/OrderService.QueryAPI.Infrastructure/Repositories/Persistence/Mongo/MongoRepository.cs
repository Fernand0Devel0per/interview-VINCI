using BuildingBlocks.Core.Interfaces;
using MongoDB.Driver;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Infrastructure.Repositories.Persistence.Mongo;

public class MongoRepository<TEntity> : IMongoRepository<TEntity> where TEntity : IEntity
{
    protected readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<TEntity>(collectionName);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task UpsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
        var options = new ReplaceOptions { IsUpsert = true };
        await _collection.ReplaceOneAsync(filter, entity, options, cancellationToken);
    }
    
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(x => x.Id, id);
        await _collection.DeleteOneAsync(filter, cancellationToken);
    }
}