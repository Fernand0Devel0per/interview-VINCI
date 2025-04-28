using Microsoft.EntityFrameworkCore;
using OrderService.CommandAPI.API.Endpoints;
using OrderService.CommandAPI.Infrastructure.Data;
using OrderService.CommandAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCommandServices(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomerEndpoints();
app.ConfigureProductEndpoints();
app.ConfigureOrderEndpoints();

app.UseHttpsRedirection();
app.Run();

