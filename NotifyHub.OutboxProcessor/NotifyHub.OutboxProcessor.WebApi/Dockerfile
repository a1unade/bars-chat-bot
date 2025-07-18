FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotifyHub.OutboxProcessor.WebApi/NotifyHub.OutboxProcessor.WebApi.csproj", "NotifyHub.OutboxProcessor.WebApi/"]
COPY ["NotifyHub.OutboxProcessor.Infrastructure/NotifyHub.OutboxProcessor.Infrastructure.csproj", "NotifyHub.OutboxProcessor.Infrastructure/"]
COPY ["NotifyHub.OutboxProcessor.Application/NotifyHub.OutboxProcessor.Application.csproj", "NotifyHub.OutboxProcessor.Application/"]
COPY ["NotifyHub.OutboxProcessor.Domain/NotifyHub.OutboxProcessor.Domain.csproj", "NotifyHub.OutboxProcessor.Domain/"]
COPY ["NotifyHub.OutboxProcessor.Persistence/NotifyHub.OutboxProcessor.Persistence.csproj", "NotifyHub.OutboxProcessor.Persistence/"]
RUN dotnet restore "NotifyHub.OutboxProcessor.WebApi/NotifyHub.OutboxProcessor.WebApi.csproj"
COPY . .
WORKDIR "/src/NotifyHub.OutboxProcessor.WebApi"
RUN dotnet build "NotifyHub.OutboxProcessor.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotifyHub.OutboxProcessor.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotifyHub.OutboxProcessor.WebApi.dll"]
