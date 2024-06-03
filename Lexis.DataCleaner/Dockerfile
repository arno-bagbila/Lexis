FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lexis.DataCleaner/Lexis.DataCleaner.csproj", "Lexis.DataCleaner/"]
RUN dotnet restore "Lexis.DataCleaner/Lexis.DataCleaner.csproj"
COPY . .
WORKDIR "/src/Lexis.DataCleaner"
RUN dotnet build "Lexis.DataCleaner.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Lexis.DataCleaner.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lexis.DataCleaner.dll"]
