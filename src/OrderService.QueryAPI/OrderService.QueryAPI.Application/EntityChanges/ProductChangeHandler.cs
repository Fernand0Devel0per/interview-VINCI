using System.Text.Json;
using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.Enums;
using OrderService.QueryAPI.Application.Common.Abstractions;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.EntityChanges;

public class ProductChangeHandler : IEntityChangeHandler
{
    private readonly IProductMongoRepository _productRepository;
    private readonly ICacheService _cacheService;

    public ProductChangeHandler(IProductMongoRepository productRepository, ICacheService cacheService)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task HandleAsync(string rawData, EntityChangeType changeType, CancellationToken cancellationToken)
    {
        var product = JsonSerializer.Deserialize<Product>(rawData);

        if (product is null)
            throw new Exception("Failed to deserialize Product.");

        switch (changeType)
        {
            case EntityChangeType.Created:
            case EntityChangeType.Updated:
                await _productRepository.UpsertAsync(product, cancellationToken);
                break;
            case EntityChangeType.Deleted:
                await _productRepository.DeleteAsync(product.Id, cancellationToken);
                break;
        }
        
        await _cacheService.RemoveAsync($"product:{product.Id}", cancellationToken);
    }
}