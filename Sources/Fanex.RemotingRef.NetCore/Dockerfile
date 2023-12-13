# https://hub.docker.com/_/microsoft-dotnet
FROM hub.nexdev.net/base/dotnet/core/sdk:7.0 AS builder
WORKDIR /Sources

COPY Sources/Fanex.RemotingRef.NetCore/Fanex.RemotingRef.NetCore.csproj ./Fanex.RemotingRef.NetCore/
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet nuget add source https://nuget2023.owgps.net/nuget/oneworks/v3/index.json
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore -r linux-x64 Fanex.RemotingRef.NetCore/Fanex.RemotingRef.NetCore.csproj

COPY Sources/Fanex.RemotingRef.NetCore ./Fanex.RemotingRef.NetCore/
WORKDIR /Sources/Fanex.RemotingRef.NetCore
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish -c release -o /app -r linux-x64 --self-contained false --no-restore

FROM hub.nexdev.net/base/dotnet/core/aspnet:7.0-alpine as base

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

# final stage/image

WORKDIR /app
COPY --from=builder /app .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.RemotingRef.NetCore.dll"]