FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# install OpenTelemetry .NET Automatic Instrumentation
ARG OTEL_VERSION=1.4.0
ADD https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v${OTEL_VERSION}/otel-dotnet-auto-install.sh otel-dotnet-auto-install.sh
RUN apt-get update && apt-get install -y unzip curl && \
    OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto" sh otel-dotnet-auto-install.sh
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Service/Directory.Packages.props", "Service/"]
COPY ["Service/Examples.Service.csproj", "Service/"]
RUN dotnet restore "./Service/Examples.Service.csproj"
COPY . .
WORKDIR "/src/Service"
RUN dotnet build "./Examples.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Examples.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Examples.Service.dll"]