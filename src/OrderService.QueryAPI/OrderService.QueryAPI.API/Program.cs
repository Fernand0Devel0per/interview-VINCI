using OrderService.QueryAPI.API.Configurations;
using OrderService.QueryAPI.API.Endpoints;
using OrderService.QueryAPI.API.Middlewares;
using OrderService.QueryAPI.Infrastructure.DependencyInjection;
using Serilog;
using BuildingBlocks.Logging.LoggingConfiguration;

var logger = SerilogConfiguration.CreateLogger();
Log.Logger = logger;

try
{
    Log.Information("Starting OrderService.QueryAPI");

    var builder = WebApplication.CreateBuilder(args);
    
    builder.WebHost.UseUrls("http://*:80");
    
    builder.Host. UseSerilog(logger);
    
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
    builder.Services.AddQueryServices(builder.Configuration);
    builder.Services.AddSwaggerConfiguration();

    var app = builder.Build();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService Query API");
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
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}