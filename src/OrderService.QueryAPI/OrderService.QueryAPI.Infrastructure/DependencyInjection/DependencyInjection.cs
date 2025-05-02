using BuildingBlocks.Caching.Abstractions;
using BuildingBlocks.Caching.Redis;
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
using StackExchange.Redis;


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

            var redisConfig = configuration.GetSection("Redis:ConnectionString").Value;
            var options = ConfigurationOptions.Parse(redisConfig);
            options.AbortOnConnectFail = false;
            
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(options));
            
            services.AddScoped<ICacheService, RedisCacheService>();
            
            services.AddScoped<ICustomerMongoRepository, CustomerMongoRepository>();
            services.AddScoped<IProductMongoRepository, ProductMongoRepository>();
            services.AddScoped<IOrderMongoRepository, OrderMongoRepository>();
            
            services.AddScoped<ICustomerQueryService, CustomerQueryService>();
            services.AddScoped<IProductQueryService, ProductQueryService>();
            services.AddScoped<IOrderQueryService, OrderQueryService>();
            
            return services;
        }
        
        public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
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
            
            var redisConfig = configuration.GetSection("Redis:ConnectionString").Value;
            var options = ConfigurationOptions.Parse(redisConfig);
            options.AbortOnConnectFail = false;
            
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(options));
            
            services.AddScoped<ICacheService, RedisCacheService>();
            
            services.AddScoped<ICustomerMongoRepository, CustomerMongoRepository>();
            services.AddScoped<IProductMongoRepository, ProductMongoRepository>();
            services.AddScoped<IOrderMongoRepository, OrderMongoRepository>();

            services.AddScoped<CustomerChangeHandler>();
            services.AddScoped<ProductChangeHandler>();
            services.AddScoped<OrderChangeHandler>();
            
            services.AddScoped<IEntityChangeDispatcher, EntityChangeDispatcher>();
            
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

            return services;
        }
    }
}
