using BuildingBlocks.Core.ApiResponses;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Application.UseCases.Customers.Mappings;
using OrderService.CommandAPI.Domain.Repositories;
using OrderService.CommandAPI.Infrastructure.Data;

namespace OrderService.CommandAPI.Application.UseCases.Customers;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly CommandDbContext _dbContext;

    public CustomerService(ICustomerRepository customerRepository, CommandDbContext dbContext)
    {
        _customerRepository = customerRepository;
        _dbContext = dbContext;
    }

    public async Task<IApiResponse> CreateCustomerAsync(CreateCustomerDto requestDto, CancellationToken cancellationToken = default)
    {
        var customer = requestDto.ToEntity();

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(customer.Id, "Customer created successfully.");
    }

    public async Task<IApiResponse> UpdateCustomerAsync(Guid id, UpdateCustomerDto requestDto, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
            return ApiResponse<string>.Fail(new List<string> { "Customer not found" }, "Not Found");

        requestDto.UpdateEntity(customer);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(customer.Id, "Customer updated successfully.");
    }

    public async Task<IApiResponse> DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
            return ApiResponse<string>.Fail(new List<string> { "Customer not found" }, "Not Found");

        await _customerRepository.DeleteAsync(id, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse<Guid>.Ok(id, "Customer deleted successfully.");
    }
}