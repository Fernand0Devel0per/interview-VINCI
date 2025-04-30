using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.RabbitMq;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.CommandAPI.Application.Common;
using OrderService.CommandAPI.Application.UseCases.Customers.Services;
using OrderService.CommandAPI.Application.UseCases.Customers.Validators;
using OrderService.CommandAPI.Application.UseCases.Orders.Services;
using OrderService.CommandAPI.Application.UseCases.Products.Services;
using OrderService.CommandAPI.Domain.Repositories;
using OrderService.CommandAPI.Infrastructure.Data;
using OrderService.CommandAPI.Infrastructure.Repositories;


namespace OrderService.CommandAPI.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommandServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDbContext<CommandDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CommandDbContext>());

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, Application.UseCases.Orders.Services.OrderService>();
            
            services.AddScoped<IEventPublisherService,EventPublisherService>();
            
            services.AddSingleton<IMessageProducer>(sp => new RabbitMqProducer(
                hostName: configuration["RabbitMq:HostName"],
                userName: configuration["RabbitMq:UserName"],
                password: configuration["RabbitMq:Password"]
            ));
            
            services.AddSingleton<IMessageConsumer>(sp => new RabbitMqConsumer(
                hostName: configuration["RabbitMq:HostName"],
                userName: configuration["RabbitMq:UserName"],
                password: configuration["RabbitMq:Password"]
            ));
            services.AddSingleton<IMessageBus, RabbitMqMessageBus>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateCustomerDtoValidator>();

            return services;
        }
    }
}
