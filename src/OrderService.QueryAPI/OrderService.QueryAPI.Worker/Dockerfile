FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src/OrderService.QueryAPI/OrderService.QueryAPI.Worker
RUN dotnet restore
RUN dotnet publish "OrderService.QueryAPI.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "OrderService.QueryAPI.Worker.dll"]
