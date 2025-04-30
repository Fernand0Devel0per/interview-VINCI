using BuildingBlocks.Caching.Abstractions;
using FluentAssertions;
using NSubstitute;
using OrderService.QueryAPI.Application.UseCases.Customers.DTOs;
using OrderService.QueryAPI.Application.UseCases.Customers.Services;
using OrderService.QueryAPI.Domain.Entities;
using OrderService.QueryAPI.Domain.Repositories;


namespace OrderService.QueryAPI.Tests.Application.UseCases.Customers;

public class CustomerQueryServiceTests
{
    private readonly ICustomerMongoRepository _repository = Substitute.For<ICustomerMongoRepository>();
    private readonly ICacheService _cacheService = Substitute.For<ICacheService>();
    private readonly CustomerQueryService _service;

    public CustomerQueryServiceTests()
    {
        _service = new CustomerQueryService(_repository, _cacheService);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnCustomer_WhenCustomerIsCached()
    {
        var customerId = Guid.NewGuid();
        var cachedCustomer = new CustomerResponseDto { Id = customerId, Name = "Cached Name", Email = "cached@example.com" };
        _cacheService.GetAsync<CustomerResponseDto>($"customer:{customerId}", Arg.Any<CancellationToken>())
            .Returns(cachedCustomer);

        var result = await _service.GetCustomerByIdAsync(customerId);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(customerId);
        await _repository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ShouldReturnNotFound_WhenCustomerDoesNotExist()
    {
        var id = Guid.NewGuid();
        _cacheService.GetAsync<CustomerResponseDto>($"customer:{id}", Arg.Any<CancellationToken>())
            .Returns((CustomerResponseDto?)null);

        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>())
            .Returns((Customer?)null);

        var result = await _service.GetCustomerByIdAsync(id);

        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain("Customer not found");
    }

    [Fact]
    public async Task GetAllCustomersAsync_ShouldReturnCustomerList()
    {
        var customers = new List<Customer>
        {
            new Customer(Guid.NewGuid(), "Alice", "alice@example.com"),
            new Customer(Guid.NewGuid(), "Bob", "bob@example.com")
        };
        _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(customers);

        var result = await _service.GetAllCustomersAsync();

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().HaveCount(2);
    }
}