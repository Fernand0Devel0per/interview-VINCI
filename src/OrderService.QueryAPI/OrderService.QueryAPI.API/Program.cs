using MongoDB.Driver;
using OrderService.QueryAPI.API.Configurations;
using OrderService.QueryAPI.API.Endpoints;
using OrderService.QueryAPI.API.Middlewares;
using OrderService.QueryAPI.Infrastructure.Data;
using OrderService.QueryAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQueryServices(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "rderService Query API");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.ConfigureCustomerEndpoints();
app.ConfigureProductEndpoints();
app.ConfigureOrderEndpoints();

app.Run();
