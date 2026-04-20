# Estágio Base: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
# Prepara diretório de logs para o requisito de Rotation Log
RUN mkdir -p /app/logs && chmod 777 /app/logs
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de Build: SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# CORREÇÃO: Copia o csproj para a raiz do WORKDIR para o restore funcionar
COPY ["NetconGeoAPI.csproj", "./"]
RUN dotnet restore "NetconGeoAPI.csproj"

# Copia o restante dos arquivos e compila
COPY . .
RUN dotnet build "NetconGeoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de Publicação
FROM build AS publish
RUN dotnet publish "NetconGeoAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio Final: Produção
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copia o dataset para a raiz da execução (essencial para o DataSeeder)
COPY dataset_v2.json .

# Define a variável de porta para compatibilidade com Cloud Run (GCP)
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "NetconGeoAPI.dll"]