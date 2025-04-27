using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Products.Dtos;
using OrderService.QueryAPI.Application.UseCases.Products.Mappings;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.UseCases.Products.Services;

public class ProductQueryService : IProductQueryService
{
    private readonly IProductMongoRepository _productRepository;

    public ProductQueryService(IProductMongoRepository productReadRepository)
    {
        _productRepository = productReadRepository;
    }

    public async Task<IApiResponse> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
            return ApiResponse<string>.Fail(new List<string> { "Product not found" }, "Not Found");

        return ApiResponse<ProductResponseDto>.Ok(product.ToResponseDto(), "Product retrieved successfully.");
    }

    public async Task<IApiResponse> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        var result = products.Select(x => x.ToResponseDto()).ToList();

        return ApiResponse<List<ProductResponseDto>>.Ok(result, "Products retrieved successfully.");
    }
}