using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.API.Configurations;
using OrderService.CommandAPI.API.Endpoints;
using OrderService.CommandAPI.API.Middlewares;
using OrderService.CommandAPI.Infrastructure.DependencyInjection;
using Serilog;
using BuildingBlocks.Logging.LoggingConfiguration;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

Log.Logger = SerilogConfiguration.CreateLogger();

try
{
    Log.Information("Starting OrderService.CommandAPI");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.WebHost.UseUrls("http://*:80");
    
    builder.Host.UseSerilog(Log.Logger);
    

    builder.Services
        .AddOpenTelemetry()
        .ConfigureResource(resource => resource
            .AddService(serviceName: Environment.GetEnvironmentVariable("SERVICE_NAME") ?? "orders-command-api"))
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddOtlpExporter(otlp =>
            {
                otlp.Endpoint = new Uri("http://lgtm:4317");
            }));
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowGateway", policy =>
        {
            policy
                .WithOrigins("http://localhost:5000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    builder.Services.AddSwaggerConfiguration();
    builder.Services.AddCommandServices(builder.Configuration);

    var app = builder.Build();
    
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService Command API");
            c.RoutePrefix = "swagger";
        });
    }
    app.UseCors("AllowGateway");
    app.UseHttpsRedirection();
    
    app.ConfigureCustomerEndpoints();
    app.ConfigureProductEndpoints();
    app.ConfigureOrderEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "OrderService.CommandAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}