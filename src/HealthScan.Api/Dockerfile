FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/HealthScan.Domain/HealthScan.Domain.csproj", "src/HealthScan.Domain/"]
COPY ["src/HealthScan.Application/HealthScan.Application.csproj", "src/HealthScan.Application/"]
COPY ["src/HealthScan.Infrastructure/HealthScan.Infrastructure.csproj", "src/HealthScan.Infrastructure/"]
COPY ["src/HealthScan.Api/HealthScan.Api.csproj", "src/HealthScan.Api/"]

RUN dotnet restore "src/HealthScan.Api/HealthScan.Api.csproj"

COPY . .
WORKDIR "/src/src/HealthScan.Api"
RUN dotnet build "HealthScan.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthScan.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthScan.Api.dll"]
