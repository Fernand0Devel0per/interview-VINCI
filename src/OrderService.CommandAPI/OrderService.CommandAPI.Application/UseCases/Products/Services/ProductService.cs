using BuildingBlocks.Core.ApiResponses;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Application.UseCases.Products.Mappings;
using OrderService.CommandAPI.Domain.Repositories;
using OrderService.CommandAPI.Infrastructure.Data;

namespace OrderService.CommandAPI.Application.UseCases.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly CommandDbContext _dbContext;
    private readonly EventPublisherService _eventPublisherService;

    public ProductService(IProductRepository productRepository, CommandDbContext dbContext, EventPublisherService eventPublisherService)
    {
        _productRepository = productRepository;
        _dbContext = dbContext;
        _eventPublisherService = eventPublisherService;
    }

    public async Task<IApiResponse> CreateProductAsync(CreateProductDto requestDto, CancellationToken cancellationToken = default)
    {
        var product = requestDto.ToEntity();

        await _productRepository.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Created,
            product,
            EventTopics.ProductChanges,
            cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(product.Id, "Product created successfully.");
    }

    public async Task<IApiResponse> UpdateProductAsync(Guid id, UpdateProductDto requestDto, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return ApiResponse<string>.Fail(new List<string> { "Product not found" }, "Not Found");

        requestDto.UpdateEntity(product);

        await _productRepository.UpdateAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Updated,
            product,
            EventTopics.ProductChanges,
            cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(product.Id, "Product updated successfully.");
    }

    public async Task<IApiResponse> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return ApiResponse<string>.Fail(new List<string> { "Product not found" }, "Not Found");

        await _productRepository.DeleteAsync(id, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Deleted,
            product,
            EventTopics.ProductChanges,
            cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(id, "Product deleted successfully.");
    }
}