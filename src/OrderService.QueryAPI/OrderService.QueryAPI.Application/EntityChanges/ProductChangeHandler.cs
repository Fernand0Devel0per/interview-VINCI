using System.Text.Json;
using BuildingBlocks.Core.Enums;
using OrderService.QueryAPI.Application.Common.Abstractions;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.EntityChanges;

public class ProductChangeHandler : IEntityChangeHandler
{
    private readonly IProductMongoRepository _productRepository;

    public ProductChangeHandler(IProductMongoRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task HandleAsync(string rawData, EntityChangeType changeType, CancellationToken cancellationToken)
    {
        var product = JsonSerializer.Deserialize<Product>(rawData);

        if (product is null)
            throw new Exception("Failed to deserialize Product.");

        switch (changeType)
        {
            case EntityChangeType.Created:
                await _productRepository.AddAsync(product, cancellationToken);
                break;
            case EntityChangeType.Updated:
                await _productRepository.UpdateAsync(product.Id, product, cancellationToken);
                break;
            case EntityChangeType.Deleted:
                await _productRepository.DeleteAsync(product.Id, cancellationToken);
                break;
        }
    }
}