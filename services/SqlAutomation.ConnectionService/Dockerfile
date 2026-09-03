FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release

WORKDIR /source

COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY global.json ./
COPY services/SqlAutomation.ConnectionService/SqlAutomation.ConnectionService.csproj services/SqlAutomation.ConnectionService/

RUN dotnet restore services/SqlAutomation.ConnectionService/SqlAutomation.ConnectionService.csproj -a $TARGETARCH

COPY services/SqlAutomation.ConnectionService/ services/SqlAutomation.ConnectionService/

WORKDIR /source/services/SqlAutomation.ConnectionService

RUN dotnet publish SqlAutomation.ConnectionService.csproj \
    -a $TARGETARCH \
    -c $BUILD_CONFIGURATION \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "SqlAutomation.ConnectionService.dll"]
