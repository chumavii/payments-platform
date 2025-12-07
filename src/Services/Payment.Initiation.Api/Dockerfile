# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# Comment this unless APP_UID is defined in your environment
# USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files to leverage layer caching
COPY ["src/Services/Payment.Initiation.Api/Payment.Initiation.Api.csproj", "src/Services/Payment.Initiation.Api/"]
COPY ["src/Shared/BuildingBlocks/BuildingBlocks.csproj", "src/Shared/BuildingBlocks/"]
COPY ["src/Shared/Contracts/Contracts.csproj", "src/Shared/Contracts/"]
COPY ["src/Shared/Domain/Domain.csproj", "src/Shared/Domain/"]

# Restore using the project path relative to current WORKDIR (/src)
RUN dotnet restore "src/Services/Payment.Initiation.Api/Payment.Initiation.Api.csproj"

# Copy the whole repo (so sources are present)
COPY . .

# IMPORTANT: after COPY ., the source tree is /src/src/...
WORKDIR "/src/src/Services/Payment.Initiation.Api"
RUN dotnet build "Payment.Initiation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/src/Services/Payment.Initiation.Api"
RUN dotnet publish "Payment.Initiation.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Initiation.Api.dll"]