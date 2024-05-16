FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/OfferCommand/OfferCommand.csproj", "OfferCommand/"]
COPY ["Services/Models/Models.csproj", "Models/"]
RUN dotnet restore "./OfferCommand/OfferCommand.csproj"

COPY Services/OfferCommand/. ./OfferCommand/
COPY Services/Models/. ./Models/
RUN dotnet build "./OfferCommand/OfferCommand.csproj" -c Release -o /app/build -nowarn:CS8618

FROM build AS publish
RUN dotnet publish "./OfferCommand/OfferCommand.csproj" -c Release -o /app/publish --self-contained false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfferCommand.dll"]