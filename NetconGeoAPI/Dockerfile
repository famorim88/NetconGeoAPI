FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Criar pasta de logs e dar permissão antes de mudar de usuário
USER root
RUN mkdir -p /app/logs && chmod 777 /app/logs
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NetconGeoAPI/NetconGeoAPI.csproj", "NetconGeoAPI/"]
RUN dotnet restore "./NetconGeoAPI/NetconGeoAPI.csproj"
COPY . .
WORKDIR "/src/NetconGeoAPI"
RUN dotnet build "./NetconGeoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NetconGeoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# IMPORTANTE: Copia o dataset para a raiz da app no container
COPY ["NetconGeoAPI/dataset_v2.json", "."]

ENTRYPOINT ["dotnet", "NetconGeoAPI.dll"]