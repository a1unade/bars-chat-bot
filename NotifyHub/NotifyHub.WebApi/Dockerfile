FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotifyHub.WebApi/NotifyHub.WebApi.csproj", "NotifyHub.WebApi/"]
COPY ["NotifyHub.Application/NotifyHub.Application.csproj", "NotifyHub.Application/"]
COPY ["NotifyHub.Domain/NotifyHub.Domain.csproj", "NotifyHub.Domain/"]
COPY ["NotifyHub.Infrastructure/NotifyHub.Infrastructure.csproj", "NotifyHub.Infrastructure/"]
COPY ["NotifyHub.Persistence/NotifyHub.Persistence.csproj", "NotifyHub.Persistence/"]
RUN dotnet restore "NotifyHub.WebApi/NotifyHub.WebApi.csproj"
COPY . .
WORKDIR "/src/NotifyHub.WebApi"
RUN dotnet build "NotifyHub.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotifyHub.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotifyHub.WebApi.dll"]
