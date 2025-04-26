using BuildingBlocks.Core.Interfaces;

namespace OrderService.CommandAPI.Domain.Entities;

public class Customer : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    public List<Order> Orders { get; private set; } = new();

    public Customer(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
    }

    private Customer() { }
}