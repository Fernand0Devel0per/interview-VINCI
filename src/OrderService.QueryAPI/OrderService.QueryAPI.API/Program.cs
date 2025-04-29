using MongoDB.Driver;
using OrderService.QueryAPI.API.Endpoints;
using OrderService.QueryAPI.Infrastructure.Data;
using OrderService.QueryAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddQueryServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.ConfigureCustomerEndpoints();
app.ConfigureProductEndpoints();
app.ConfigureOrderEndpoints();

app.Run();
