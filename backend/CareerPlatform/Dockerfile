FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["CareerPlatform.API/CareerPlatform.API.csproj", "CareerPlatform.API/"]
COPY ["CareerPlatform.Application/CareerPlatform.Application.csproj", "CareerPlatform.Application/"]
COPY ["CareerPlatform.Domain/CareerPlatform.Domain.csproj", "CareerPlatform.Domain/"]
COPY ["CareerPlatform.Infrastructure/CareerPlatform.Infrastructure.csproj", "CareerPlatform.Infrastructure/"]
COPY ["CareerPlatform.Tests/CareerPlatform.Tests.csproj", "CareerPlatform.Tests/"]

RUN dotnet restore "CareerPlatform.API/CareerPlatform.API.csproj"
COPY . .
RUN dotnet publish "CareerPlatform.API/CareerPlatform.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CareerPlatform.API.dll"]
