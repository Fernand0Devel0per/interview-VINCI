using BuildingBlocks.Core.ApiResponses;
using BuildingBlocks.Core.Enums;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Application.UseCases.Customers.Mappings;
using OrderService.CommandAPI.Domain.Repositories;

namespace OrderService.CommandAPI.Application.UseCases.Customers.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventPublisherService _eventPublisherService;

    public CustomerService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, IEventPublisherService eventPublisherService)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _eventPublisherService = eventPublisherService;
    }

    public async Task<IApiResponse> CreateCustomerAsync(CreateCustomerDto requestDto, CancellationToken cancellationToken = default)
    {
        var customer = requestDto.ToEntity();

        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Created,
            customer,
            cancellationToken: cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(customer.Id, "Customer created successfully.");
    }

    public async Task<IApiResponse> UpdateCustomerAsync(Guid id, UpdateCustomerDto requestDto, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
            return ApiResponse<string>.Fail(new List<string> { "Customer not found" }, "Not Found");

        requestDto.UpdateEntity(customer);

        await _customerRepository.UpdateAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Updated,
            customer,
            cancellationToken: cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(customer.Id, "Customer updated successfully.");
    }

    public async Task<IApiResponse> DeleteCustomerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);

        if (customer is null)
            return ApiResponse<string>.Fail(new List<string> { "Customer not found" }, "Not Found");

        await _customerRepository.DeleteAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        await _eventPublisherService.PublishEntityChangedEventAsync(
            EntityChangeType.Deleted,
            customer,
            cancellationToken: cancellationToken
        );
        
        return ApiResponse<Guid>.Ok(id, "Customer deleted successfully.");
    }
}