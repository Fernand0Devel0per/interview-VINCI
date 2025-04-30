using System;
using FluentAssertions;
using OrderService.CommandAPI.Domain.Entities;
using Xunit;

namespace OrderService.CommandAPI.Tests.Domain.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_Should_Set_Properties_Correctly()
    {
        var name = "Keyboard";
        var price = 120.50m;
        
        var product = new Product(name, price);
        
        product.Id.Should().NotBeEmpty();
        product.Name.Should().Be(name);
        product.Price.Should().Be(price);
    }

    [Fact]
    public void UpdateName_Should_Change_Name_When_Valid()
    {
        var product = new Product("Old Name", 100m);
        var newName = "New Name";
        
        product.UpdateName(newName);
        
        product.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateName_Should_Not_Change_Name_When_Invalid(string? invalidName)
    {
        var originalName = "Original";
        var product = new Product(originalName, 100m);
        
        product.UpdateName(invalidName);
        
        product.Name.Should().Be(originalName);
    }

    [Fact]
    public void UpdatePrice_Should_Change_Price_When_Valid()
    {
        var product = new Product("Item", 50m);
        var newPrice = 99.99m;
        
        product.UpdatePrice(newPrice);
        
        product.Price.Should().Be(newPrice);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void UpdatePrice_Should_Not_Change_Price_When_Invalid(decimal invalidPrice)
    {
        var originalPrice = 80m;
        var product = new Product("Item", originalPrice);
        
        product.UpdatePrice(invalidPrice);
        
        product.Price.Should().Be(originalPrice);
    }
}