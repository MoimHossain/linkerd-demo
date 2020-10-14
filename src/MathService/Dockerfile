#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
#ENV ASPNETCORE_URLS=https://+:443
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MathService/MathService.csproj", "MathService/"]
COPY ["Octolamp.Contracts/Octolamp.Contracts.csproj", "Octolamp.Contracts/"]
RUN dotnet restore "MathService/MathService.csproj"
COPY . .
WORKDIR "/src/MathService"
RUN dotnet build "MathService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MathService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MathService.dll"]