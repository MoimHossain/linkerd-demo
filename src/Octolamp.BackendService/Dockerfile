FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src

COPY ./Octolamp.BackendService/Octolamp.BackendService.csproj ./src/Octolamp.BackendService/
COPY ./Octolamp.Contracts/Octolamp.Contracts.csproj ./src/Octolamp.Contracts/

WORKDIR src/Octolamp.BackendService
RUN dotnet restore

WORKDIR /src
COPY . .

RUN dotnet build Octolamp.BackendService/Octolamp.BackendService.csproj -c Release -o /app/build
RUN dotnet publish Octolamp.BackendService/Octolamp.BackendService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "Octolamp.BackendService.dll" ]
