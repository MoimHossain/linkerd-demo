FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src

COPY ./Octolamp.Frontend/Octolamp.Frontend.csproj ./src/Octolamp.Frontend/
COPY ./Octolamp.Contracts/Octolamp.Contracts.csproj ./src/Octolamp.Contracts/

WORKDIR src/Octolamp.Frontend
RUN dotnet restore

WORKDIR /src
COPY . .

RUN dotnet build Octolamp.Frontend/Octolamp.Frontend.csproj -c Release -o /app/build
RUN dotnet publish Octolamp.Frontend/Octolamp.Frontend.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "Octolamp.Frontend.dll" ]
