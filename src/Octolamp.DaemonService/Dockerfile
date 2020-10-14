FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Octolamp.DaemonService/Octolamp.DaemonService.csproj", "Octolamp.DaemonService/"]
COPY ["Octolamp.Contracts/Octolamp.Contracts.csproj", "Octolamp.Contracts/"]
RUN dotnet restore "Octolamp.DaemonService/Octolamp.DaemonService.csproj"
COPY . .
WORKDIR "/src/Octolamp.DaemonService"
RUN dotnet build "Octolamp.DaemonService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Octolamp.DaemonService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Octolamp.DaemonService.dll"]