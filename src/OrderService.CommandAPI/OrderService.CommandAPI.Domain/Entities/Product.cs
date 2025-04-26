using BuildingBlocks.Core.Interfaces;

namespace OrderService.CommandAPI.Domain.Entities;

public class Product : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }

    private Product() { }

    public void UpdateName(string? name)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            Name = name;
        }
    }

    public void UpdatePrice(decimal price)
    {
        if (price > 0)
        {
            Price = price;
        }
    }
}