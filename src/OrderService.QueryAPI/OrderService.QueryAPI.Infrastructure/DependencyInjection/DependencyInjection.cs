using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OrderService.QueryAPI.Application.UseCases.Customers.Services;
using OrderService.QueryAPI.Application.UseCases.Orders.Services;
using OrderService.QueryAPI.Application.UseCases.Products.Services;
using OrderService.QueryAPI.Domain.Repositories;
using OrderService.QueryAPI.Infrastructure.Repositories.Persistence.Mongo;
using OrderService.QueryAPI.Application.Common.Abstractions;
using BuildingBlocks.Messaging.Abstractions;
using BuildingBlocks.Messaging.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using OrderService.QueryAPI.Application.EntityChanges;


namespace OrderService.QueryAPI.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddQueryServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.AddSingleton<IMongoClient>(sp =>
            {
                var connectionString = configuration["MongoDb:ConnectionString"];
                return new MongoClient(connectionString);
            });

            services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var databaseName = configuration["MongoDb:DatabaseName"];
                return client.GetDatabase(databaseName);
            });

            //services.AddStackExchangeRedisCache(options => options.Configuration = configuration["Redis:ConnectionString"]);
            
            services.AddScoped<ICustomerMongoRepository, CustomerMongoRepository>();
            services.AddScoped<IProductMongoRepository, ProductMongoRepository>();
            services.AddScoped<IOrderMongoRepository, OrderMongoRepository>();
            
            services.AddScoped<ICustomerQueryService, CustomerQueryService>();
            services.AddScoped<IProductQueryService, ProductQueryService>();
            services.AddScoped<IOrderQueryService, OrderQueryService>();
            
            services.AddScoped<IEntityChangeDispatcher, EntityChangeDispatcher>();
            
            services.AddSingleton<IMessageConsumer>(sp => new RabbitMqConsumer(
                hostName: configuration["RabbitMq:HostName"],
                userName: configuration["RabbitMq:UserName"],
                password: configuration["RabbitMq:Password"]
            ));
            services.AddSingleton<IMessageBus, RabbitMqMessageBus>();


            return services;
        }
    }
}
