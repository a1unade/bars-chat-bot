FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotifyHub.NotificationService.WebApi/NotifyHub.NotificationService.WebApi.csproj", "NotifyHub.NotificationService.WebApi/"]
COPY ["NotifyHub.NotificationService.Application/NotifyHub.NotificationService.Application.csproj", "NotifyHub.NotificationService.Application/"]
COPY ["NotifyHub.NotificationService.Domain/NotifyHub.NotificationService.Domain.csproj", "NotifyHub.NotificationService.Domain/"]
COPY ["NotifyHub.NotificationService.Infrastructure/NotifyHub.NotificationService.Infrastructure.csproj", "NotifyHub.NotificationService.Infrastructure/"]
COPY ["NotifyHub.NotificationService.Persistence/NotifyHub.NotificationService.Persistence.csproj", "NotifyHub.NotificationService.Persistence/"]
RUN dotnet restore "NotifyHub.NotificationService.WebApi/NotifyHub.NotificationService.WebApi.csproj"
COPY . .
WORKDIR "/src/NotifyHub.NotificationService.WebApi"
RUN dotnet build "NotifyHub.NotificationService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotifyHub.NotificationService.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotifyHub.NotificationService.WebApi.dll"]
