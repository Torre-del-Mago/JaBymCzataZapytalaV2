FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/Trip/Trip.csproj", "Trip/"]
COPY ["Services/Models/Models.csproj", "Models/"]
RUN dotnet restore "./Trip/Trip.csproj"

COPY Services/Trip/. ./Trip/
COPY Services/Models/. ./Models/
RUN dotnet build "./Trip/Trip.csproj" -c Release -o /app/build -nowarn:CS8618

FROM build AS publish
RUN dotnet publish "./Trip/Trip.csproj" -c Release -o /app/publish --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trip.dll"]