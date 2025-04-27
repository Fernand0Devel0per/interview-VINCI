using BuildingBlocks.Core.ApiResponses;

namespace OrderService.QueryAPI.Application.UseCases.Customers.Services;

public interface ICustomerQueryService
{
    Task<IApiResponse> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IApiResponse> GetAllCustomersAsync(CancellationToken cancellationToken = default);
}