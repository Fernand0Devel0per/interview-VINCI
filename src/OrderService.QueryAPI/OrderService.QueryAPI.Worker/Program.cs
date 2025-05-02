using OrderService.QueryAPI.Infrastructure.DependencyInjection;
using Serilog;
using BuildingBlocks.Logging.LoggingConfiguration;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OrderService.QueryAPI.Worker;

Log.Logger = SerilogConfiguration.CreateLogger();

try
{
    Log.Information("Starting OrderService.QueryAPI.Worker");

    var builder = Host.CreateDefaultBuilder(args)
        .UseSerilog(Log.Logger)
        .ConfigureServices((hostContext, services) =>
        {
            IConfiguration configuration = hostContext.Configuration;
            
            services
                .AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService(serviceName: Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "orders-worker"))
                .WithMetrics(metrics =>
                {
                    metrics
                        .AddRuntimeInstrumentation()
                        .AddOtlpExporter(otlp =>
                        {
                            otlp.Endpoint = new Uri("http://lgtm:4317");
                        });
                })
                .WithTracing(tracing =>
                {
                    tracing
                        .AddSource("OrderService.Worker")
                        .AddOtlpExporter(otlp =>
                        {
                            otlp.Endpoint = new Uri("http://lgtm:4317");
                        });
                });

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