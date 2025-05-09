using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Domain.Entities;

namespace OrderService.CommandAPI.Infrastructure.Data;

public class CommandDbContext : DbContext, IUnitOfWork
{
    public CommandDbContext(DbContextOptions<CommandDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });

        modelBuilder.Entity<OrderProduct>()
            .HasOne<Order>()
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(op => op.ProductId);
    
        modelBuilder.Entity<Order>()
            .Ignore(o => o.Products);
    }
}