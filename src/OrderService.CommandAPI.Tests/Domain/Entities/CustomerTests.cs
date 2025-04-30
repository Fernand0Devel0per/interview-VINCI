using System;
using FluentAssertions;
using OrderService.CommandAPI.Domain.Entities;
using Xunit;

namespace OrderService.CommandAPI.Tests.Domain.Entities;

public class CustomerTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        var name = "John Doe";
        var email = "john@example.com";
        
        var customer = new Customer(name, email);
        
        customer.Id.Should().NotBeEmpty();
        customer.Name.Should().Be(name);
        customer.Email.Should().Be(email);
    }

    [Fact]
    public void UpdateName_Should_Change_Name_When_Valid()
    {
        var customer = new Customer("Initial", "email@example.com");
        var newName = "Updated Name";
        
        customer.UpdateName(newName);
        
        customer.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_Should_Not_Change_Name_When_Invalid(string? newName)
    {
        var originalName = "Original Name";
        var customer = new Customer(originalName, "email@example.com");
        
        customer.UpdateName(newName);
        
        customer.Name.Should().Be(originalName);
    }

    [Fact]
    public void UpdateEmail_Should_Change_Email_When_Valid()
    {
        var customer = new Customer("John", "initial@example.com");
        var newEmail = "new@example.com";
        
        customer.UpdateEmail(newEmail);
        
        customer.Email.Should().Be(newEmail);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateEmail_Should_Not_Change_Email_When_Invalid(string? newEmail)
    {
        // Arrange
        var originalEmail = "original@example.com";
        var customer = new Customer("John", originalEmail);

        // Act
        customer.UpdateEmail(newEmail);

        // Assert
        customer.Email.Should().Be(originalEmail);
    }
}
