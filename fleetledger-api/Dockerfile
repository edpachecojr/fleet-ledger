FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Build.props Directory.Packages.props .editorconfig ./
COPY src/FleetLedger.Core/FleetLedger.Core.csproj src/FleetLedger.Core/
COPY src/FleetLedger.Api/FleetLedger.Api.csproj src/FleetLedger.Api/
RUN dotnet restore src/FleetLedger.Api/FleetLedger.Api.csproj

COPY src/ src/
RUN dotnet publish src/FleetLedger.Api/FleetLedger.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "FleetLedger.Api.dll"]
