using OrderService.QueryAPI.Infrastructure.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        
        services.AddWorkerServices(configuration);
        
        services.AddHostedService<Worker>();
    });

await builder.RunConsoleAsync();