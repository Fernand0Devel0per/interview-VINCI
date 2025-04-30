using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.API.Configurations;
using OrderService.CommandAPI.API.Endpoints;
using OrderService.CommandAPI.API.Middlewares;
using OrderService.CommandAPI.Infrastructure.DependencyInjection;
using Serilog;
using BuildingBlocks.Logging.LoggingConfiguration;

Log.Logger = SerilogConfiguration.CreateLogger();

try
{
    Log.Information("Starting OrderService.CommandAPI");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog(Log.Logger);
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowGateway", policy =>
        {
            policy
                .WithOrigins("http://localhost:5000") // ou AllowAnyOrigin() se estiver testando
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