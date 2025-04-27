using BuildingBlocks.Core.ApiResponses;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;

namespace OrderService.CommandAPI.Application.UseCases.Customers.Services;

public interface ICustomerService
{
    Task<IApiResponse> CreateCustomerAsync(CreateCustomerDto requestDto, CancellationToken cancellationToken = default);
    Task<IApiResponse> UpdateCustomerAsync(Guid id, UpdateCustomerDto requestDto, CancellationToken cancellationToken = default);
    Task<IApiResponse> DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default);
}