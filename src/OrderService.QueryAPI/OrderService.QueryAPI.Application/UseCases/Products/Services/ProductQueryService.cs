using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Products.Dtos;
using OrderService.QueryAPI.Application.UseCases.Products.Mappings;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.UseCases.Products.Services;

public class ProductQueryService : IProductQueryService
{
    private readonly IProductMongoRepository _productRepository;
    private readonly ICacheService _cacheService;

    public ProductQueryService(IProductMongoRepository productRepository, ICacheService cacheService)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task<ApiResponse<ProductResponseDto>> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"product:{id}";

        var cachedProduct = await _cacheService.GetAsync<ProductResponseDto>(cacheKey, cancellationToken);
        if (cachedProduct is not null)
            return ApiResponse<ProductResponseDto>.Ok(cachedProduct);

        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return ApiResponse<ProductResponseDto>.Fail(new List<string> { "Product not found" }, "Not Found");

        var responseDto = product.ToResponseDto();
        await _cacheService.SetAsync(cacheKey, responseDto, TimeSpan.FromMinutes(10), cancellationToken);

        return ApiResponse<ProductResponseDto>.Ok(responseDto, "Product retrieved successfully.");
    }

    public async Task<ApiResponse<List<ProductResponseDto>>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        var result = products.Select(x => x.ToResponseDto()).ToList();

        return ApiResponse<List<ProductResponseDto>>.Ok(result, "Products retrieved successfully.");
    }
}