using BuildingBlocks.Core.ApiResponses;
using OrderService.QueryAPI.Application.UseCases.Customers.DTOs;

namespace OrderService.QueryAPI.Application.UseCases.Customers.Services;

public interface ICustomerQueryService
{
    Task<ApiResponse<CustomerResponseDto>> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ApiResponse<List<CustomerResponseDto>>> GetAllCustomersAsync(CancellationToken cancellationToken = default);
}