using BuildingBlocks.Core.ApiResponses;
using BuildingBlocks.Core.Enums;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Products.DTOs;
using OrderService.CommandAPI.Application.UseCases.Products.Mappings;
using OrderService.CommandAPI.Domain.Repositories;

namespace OrderService.CommandAPI.Application.UseCases.Products.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisherService _eventPublisherService;

    public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork, IEventPublisherService eventPublisherService)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _eventPublisherService = eventPublisherService;
    }

    public async Task<IApiResponse> CreateProductAsync(CreateProductDto requestDto, CancellationToken cancellationToken = default)
    {
        var product = requestDto.ToEntity();
        await _productRepository.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Created,
            product,
            cancellationToken: cancellationToken
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Updated,
            product,
            cancellationToken: cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(product.Id, "Product updated successfully.");
    }

    public async Task<IApiResponse> DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return ApiResponse<string>.Fail(new List<string> { "Product not found" }, "Not Found");

        await _productRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Deleted,
            product,
            cancellationToken: cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(id, "Product deleted successfully.");
    }
}