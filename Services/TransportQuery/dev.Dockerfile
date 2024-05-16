FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/TransportQuery/TransportQuery.csproj", "TransportQuery/"]
COPY ["Services/Models/Models.csproj", "Models/"]
RUN dotnet restore "./TransportQuery/TransportQuery.csproj"

COPY Services/TransportQuery/. ./TransportQuery/
COPY Services/Models/. ./Models/
RUN dotnet build "./TransportQuery/TransportQuery.csproj" -c Release -o /app/build -nowarn:CS8618

FROM build AS publish
RUN dotnet publish "./TransportQuery/TransportQuery.csproj" -c Release -o /app/publish --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransportQuery.dll"]