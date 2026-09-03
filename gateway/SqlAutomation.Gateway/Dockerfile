FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG TARGETARCH
ARG BUILD_CONFIGURATION=Release

WORKDIR /source

COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY global.json ./
COPY gateway/SqlAutomation.Gateway/SqlAutomation.Gateway.csproj gateway/SqlAutomation.Gateway/

RUN dotnet restore gateway/SqlAutomation.Gateway/SqlAutomation.Gateway.csproj -a $TARGETARCH

COPY gateway/SqlAutomation.Gateway/ gateway/SqlAutomation.Gateway/

WORKDIR /source/gateway/SqlAutomation.Gateway

RUN dotnet publish SqlAutomation.Gateway.csproj \
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

ENTRYPOINT ["dotnet", "SqlAutomation.Gateway.dll"]
