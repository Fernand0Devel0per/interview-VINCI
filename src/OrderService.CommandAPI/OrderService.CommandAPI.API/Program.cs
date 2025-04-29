using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.API.Configurations;
using OrderService.CommandAPI.API.Endpoints;
using OrderService.CommandAPI.Infrastructure.Data;
using OrderService.CommandAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfiguration();
builder.Services.AddCommandServices(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService Command API");
        c.RoutePrefix = "swagger";
    });
}

app.ConfigureCustomerEndpoints();
app.ConfigureProductEndpoints();
app.ConfigureOrderEndpoints();

app.UseHttpsRedirection();
app.Run();

