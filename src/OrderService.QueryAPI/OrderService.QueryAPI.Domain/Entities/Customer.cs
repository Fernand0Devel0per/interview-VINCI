using BuildingBlocks.Core.Interfaces;

namespace OrderService.QueryAPI.Domain.Entities;

public class Customer : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    public Customer(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }
    
    private Customer() { }
}