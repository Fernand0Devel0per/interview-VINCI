using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace OrderService.QueryAPI.Infrastructure.Data;

public class MongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        var databaseName = configuration.GetSection("MongoSettings:DatabaseName").Value;

        var client = new MongoClient(connectionString);
        Database = client.GetDatabase(databaseName);
    }
}