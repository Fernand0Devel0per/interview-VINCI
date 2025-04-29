using OrderService.QueryAPI.Infrastructure.DependencyInjection;
using Serilog;
using BuildingBlocks.Logging.LoggingConfiguration;

Log.Logger = SerilogConfiguration.CreateLogger();

try
{
    Log.Information("Starting OrderService.QueryAPI.Worker");

    var builder = Host.CreateDefaultBuilder(args)
        .UseSerilog(Log.Logger)
        .ConfigureServices((hostContext, services) =>
        {
            IConfiguration configuration = hostContext.Configuration;
            services.AddWorkerServices(configuration);
            services.AddHostedService<Worker>();
        });

    await builder.RunConsoleAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "OrderService.QueryAPI.Worker terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}