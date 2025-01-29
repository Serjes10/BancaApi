FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["BancaApi.csproj", "BancaApi/"]
RUN dotnet restore "BancaApi/BancaApi.csproj"

WORKDIR "/src/BancaApi"
COPY . . 
RUN dotnet build "BancaApi.csproj" -c Release -o /app/build
RUN dotnet publish "BancaApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN apt-get update && apt-get install -y sqlite3

WORKDIR /app

COPY --from=build /app/publish . 

COPY BancaDB.db /app/data/BancaDB.db

COPY appsettings.json /app/appsettings.json

RUN sed -i 's|"Data Source=BancaDB.db"|"Data Source=/app/data/BancaDB.db"|' /app/appsettings.json

ENV ASPNETCORE_URLS=http://+:5076
ENV DB_CONNECTION_STRING="Data Source=/app/data/BancaDB.db"

EXPOSE 5076

ENTRYPOINT ["dotnet", "BancaApi.dll"]
