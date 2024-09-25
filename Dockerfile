# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lexis/LexisApi.csproj", "Lexis/"]
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "./Lexis/LexisApi.csproj"
COPY . .
WORKDIR "/src/Lexis"
RUN dotnet build "./LexisApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LexisApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the appsettings.json file to the correct location
COPY ["Lexis/Configuration/appsettings.json", "./Configuration/appsettings.json"]

ENTRYPOINT ["dotnet", "LexisApi.dll"]