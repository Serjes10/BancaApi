# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["BancaApi.csproj", "BancaApi/"]
RUN dotnet restore "BancaApi/BancaApi.csproj"

WORKDIR "/src/BancaApi"
COPY . . 
RUN dotnet build "BancaApi.csproj" -c Release -o /app/build
RUN dotnet publish "BancaApi.csproj" -c Release -o /app/publish

# Etapa final para producción
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Instalar SQLite en el contenedor final
RUN apt-get update && apt-get install -y sqlite3

WORKDIR /app

# Copiar la aplicación publicada
COPY --from=build /app/publish . 

# Copiar la base de datos
COPY BancaDB.db /app/data/BancaDB.db

# Copiar configuración
COPY appsettings.json /app/appsettings.json

# Modificar la cadena de conexión en el archivo JSON (opcional)
RUN sed -i 's|"Data Source=BancaDB.db"|"Data Source=/app/data/BancaDB.db"|' /app/appsettings.json

# Variables de entorno
ENV ASPNETCORE_URLS=http://+:5076
ENV DB_CONNECTION_STRING="Data Source=/app/data/BancaDB.db"

EXPOSE 5076

ENTRYPOINT ["dotnet", "BancaApi.dll"]
