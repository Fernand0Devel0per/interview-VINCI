using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Customers.DTOs;
using OrderService.QueryAPI.Application.UseCases.Customers.Mappings;
using OrderService.QueryAPI.Domain.Repositories;

namespace OrderService.QueryAPI.Application.UseCases.Customers.Services;

public class CustomerQueryService : ICustomerQueryService
{
    private readonly ICustomerMongoRepository _customerRepository;

    public CustomerQueryService(ICustomerMongoRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<IApiResponse> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
            return ApiResponse<string>.Fail(new List<string> { "Customer not found" }, "Not Found");

        return ApiResponse<CustomerResponseDto>.Ok(customer.ToResponseDto(), "Customer retrieved successfully.");
    }

    public async Task<IApiResponse> GetAllCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetAllAsync(cancellationToken);

        var result = customers.Select(x => x.ToResponseDto()).ToList();

        return ApiResponse<List<CustomerResponseDto>>.Ok(result, "Customers retrieved successfully.");
    }
}