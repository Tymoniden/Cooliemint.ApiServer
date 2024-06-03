#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0.3-bookworm-slim AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
ENV COOLIEMINT_VERSION=1.2.0
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0.3-bookworm-slim AS build
WORKDIR /src
COPY ["Cooliemint.ApiServer.csproj", "."]
RUN dotnet restore "./Cooliemint.ApiServer.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Cooliemint.ApiServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cooliemint.ApiServer.csproj" --arch arm64 --os linux -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cooliemint.ApiServer.dll"]