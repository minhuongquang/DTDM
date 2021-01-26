#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["DTDM.csproj", ""]
COPY ["../Dockersample/Dockersample.csproj", "../Dockersample/"]
RUN dotnet restore "./DTDM.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "DTDM.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DTDM.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DTDM.dll"]

FROM       ubuntu:18.04
RUN        apt-get install -y redis-server
EXPOSE     6379
ENTRYPOINT ["/usr/bin/redis-server"]
