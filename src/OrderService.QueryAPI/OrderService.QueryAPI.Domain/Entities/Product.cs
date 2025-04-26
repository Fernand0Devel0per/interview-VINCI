using BuildingBlocks.Core.Interfaces;

namespace OrderService.QueryAPI.Domain.Entities;

public class Product : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public Product(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
    
    private Product() { }
}