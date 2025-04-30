using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Customers.DTOs;
using OrderService.QueryAPI.Application.UseCases.Customers.Mappings;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.UseCases.Customers.Services;

public class CustomerQueryService : ICustomerQueryService
{
    private readonly ICustomerMongoRepository _customerRepository;
    private readonly ICacheService _cacheService;

    public CustomerQueryService(ICustomerMongoRepository customerRepository, ICacheService cacheService)
    {
        _customerRepository = customerRepository;
        _cacheService = cacheService;
    }

    public async Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string cacheKey = $"customer:{id}";

        var cachedCustomer = await _cacheService.GetAsync<CustomerResponseDto>(cacheKey, cancellationToken);
        if (cachedCustomer is not null)
            return ApiResponse<CustomerResponseDto>.Ok(cachedCustomer);
        
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        
        if (customer is null)
            return ApiResponse<CustomerResponseDto>.Fail(new List<string> { "Customer not found" }, "Not Found");

        var responseDto = customer.ToResponseDto();
        await _cacheService.SetAsync(cacheKey, responseDto, TimeSpan.FromMinutes(10), cancellationToken);
        
        if (customer is null)
            return ApiResponse<CustomerResponseDto>.Fail(new List<string> { "Customer not found" }, "Not Found");

        return ApiResponse<CustomerResponseDto>.Ok(customer.ToResponseDto(), "Customer retrieved successfully.");
    }

    public async Task<ApiResponse<List<CustomerResponseDto>>> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);

        var result = customers.Select(x => x.ToResponseDto()).ToList();

        return ApiResponse<List<CustomerResponseDto>>.Ok(result, "Customers retrieved successfully.");
    }
}