# https://hub.docker.com/_/microsoft-dotnet
FROM hub.nexdev.net/base/dotnet/core/sdk:7.0 AS builder
WORKDIR /Sources

COPY Demo/Fanex.RemotingRef.Demo/Fanex.RemotingRef.Demo.csproj ./Fanex.RemotingRef.Demo/
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -r linux-x64 Fanex.RemotingRef.Demo/Fanex.RemotingRef.Demo.csproj

COPY Demo/Fanex.RemotingRef.Demo ./Fanex.RemotingRef.Demo/
WORKDIR /Sources/Fanex.RemotingRef.Demo
RUN dotnet publish -c release -o /app -r linux-x64 --self-contained false --no-restore

FROM hub.nexdev.net/base/dotnet/core/aspnet:7.0-alpine AS base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
#

# final stage/image

WORKDIR /app
COPY --from=builder /app .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.RemotingRef.Demo.dll"]