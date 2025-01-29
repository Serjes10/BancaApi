FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["BancaApi.csproj", "BancaApi/"]
RUN dotnet restore "BancaApi/BancaApi.csproj"

WORKDIR "/src/BancaApi"
COPY . . 
RUN dotnet build "BancaApi.csproj" -c Release -o /app/build

RUN dotnet publish "BancaApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5076

COPY --from=build /app/publish .

RUN ls /app/

COPY BancaDB.db /app/data/BancaDB.db

ENV DB_CONNECTION_STRING="Data Source=/app/data/BancaDB.db"

EXPOSE 5076

ENTRYPOINT ["dotnet", "BancaApi.dll"]
