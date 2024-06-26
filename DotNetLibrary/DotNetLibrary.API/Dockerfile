﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DotNetLibrary.API/DotNetLibrary.API.csproj", "DotNetLibrary.API/"]
RUN dotnet restore "DotNetLibrary.API/DotNetLibrary.API.csproj"
COPY . .
WORKDIR "/src/DotNetLibrary.API"
RUN dotnet build "DotNetLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DotNetLibrary.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY DotNetLibrary.API/appsettings.json .
ENTRYPOINT ["dotnet", "DotNetLibrary.API.dll"]
