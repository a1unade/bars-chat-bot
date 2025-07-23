FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotifyHub.TelegramBot/NotifyHub.TelegramBot.csproj", "NotifyHub.TelegramBot/"]
COPY ["NotifyHub.TelegramBot.Application/NotifyHub.TelegramBot.Application.csproj", "NotifyHub.TelegramBot.Application/"]
COPY ["NotifyHub.TelegramBot.Domain/NotifyHub.TelegramBot.Domain.csproj", "NotifyHub.TelegramBot.Domain/"]
COPY ["NotifyHub.TelegramBot.Infrastructure/NotifyHub.TelegramBot.Infrastructure.csproj", "NotifyHub.TelegramBot.Infrastructure/"]
COPY ["NotifyHub.TelegramBot.Persistence/NotifyHub.TelegramBot.Persistence.csproj", "NotifyHub.TelegramBot.Persistence/"]
RUN dotnet restore "NotifyHub.TelegramBot/NotifyHub.TelegramBot.csproj"
COPY . .
WORKDIR "/src/NotifyHub.TelegramBot"
RUN dotnet build "NotifyHub.TelegramBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotifyHub.TelegramBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotifyHub.TelegramBot.dll"]
