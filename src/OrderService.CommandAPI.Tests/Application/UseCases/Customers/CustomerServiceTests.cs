using BuildingBlocks.Core.ApiResponses;
using BuildingBlocks.Core.Enums;
using FluentAssertions;
using NSubstitute;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Customers.DTOs;
using OrderService.CommandAPI.Application.UseCases.Customers.Services;
using OrderService.CommandAPI.Domain.Entities;
using OrderService.CommandAPI.Domain.Repositories;

namespace OrderService.CommandAPI.Tests.Application.UseCases.Customers;

public class CustomerServiceTests
{
    private readonly ICustomerRepository _repository = Substitute.For<ICustomerRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IEventPublisherService _eventPublisher = Substitute.For<IEventPublisherService>();
    private readonly CustomerService _service;

    public CustomerServiceTests()
    {
        _service = new CustomerService(_repository, _unitOfWork, _eventPublisher);
    }

    [Fact]
    public async Task CreateCustomerAsync_Should_Create_And_Publish_Event()
    {
        // Arrange
        var dto = new CreateCustomerDto { Name = "John", Email = "john@example.com" };

        // Act
        var result = await _service.CreateCustomerAsync(dto);

        // Assert
        result.Should().BeOfType<ApiResponse<Guid>>();
        await _repository.Received(1).AddAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishEntityChangedEventAsync(
            EntityChangeType.Created, Arg.Any<Customer>(),Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateCustomerAsync_Should_Return_NotFound_When_Customer_Does_Not_Exist()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        // Act
        var result = await _service.UpdateCustomerAsync(id, new UpdateCustomerDto());

        // Assert
        result.Should().BeOfType<ApiResponse<string>>()
              .Which.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateCustomerAsync_Should_Update_And_Publish_Event()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customer = new Customer("Old", "old@email.com");
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(customer);
        var dto = new UpdateCustomerDto { Name = "New", Email = "new@email.com" };

        // Act
        var result = await _service.UpdateCustomerAsync(id, dto);

        // Assert
        result.Should().BeOfType<ApiResponse<Guid>>();
        customer.Name.Should().Be("New");
        customer.Email.Should().Be("new@email.com");

        await _repository.Received(1).UpdateAsync(customer, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishEntityChangedEventAsync(
            EntityChangeType.Updated, customer, Arg.Any<string>(),Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteCustomerAsync_Should_Return_NotFound_When_Customer_Does_Not_Exist()
    {
        var id = Guid.NewGuid();
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((Customer?)null);

        var result = await _service.DeleteCustomerAsync(id);

        result.Should().BeOfType<ApiResponse<string>>()
              .Which.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteCustomerAsync_Should_Delete_And_Publish_Event()
    {
        var id = Guid.NewGuid();
        var customer = new Customer("Test", "test@email.com");
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns(customer);

        var result = await _service.DeleteCustomerAsync(id);

        result.Should().BeOfType<ApiResponse<Guid>>();
        await _repository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishEntityChangedEventAsync(
            EntityChangeType.Deleted, customer, Arg.Any<string>(),Arg.Any<CancellationToken>());
    }
}
