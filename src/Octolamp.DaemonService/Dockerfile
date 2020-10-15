FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src

COPY ./Octolamp.DaemonService/Octolamp.DaemonService.csproj ./src/Octolamp.DaemonService/
COPY ./Octolamp.Contracts/Octolamp.Contracts.csproj ./src/Octolamp.Contracts/

WORKDIR src/Octolamp.DaemonService
RUN dotnet restore

WORKDIR /src
COPY . .

RUN dotnet build Octolamp.DaemonService/Octolamp.DaemonService.csproj -c Release -o /app/build
RUN dotnet publish Octolamp.DaemonService/Octolamp.DaemonService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "Octolamp.DaemonService.dll" ]
