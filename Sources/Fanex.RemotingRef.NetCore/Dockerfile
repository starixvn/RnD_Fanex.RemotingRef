# https://hub.docker.com/_/microsoft-dotnet
FROM hub.nexdev.net/base/dotnet/core/sdk:7.0 AS builder
WORKDIR /Sources

COPY Sources/Fanex.RemotingRef.NetCore/Fanex.RemotingRef.NetCore.csproj ./Fanex.RemotingRef.NetCore/
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet nuget add source https://nuget.owdev.net/api/v2
RUN dotnet restore -r linux-x64 Fanex.RemotingRef.NetCore/Fanex.RemotingRef.NetCore.csproj

COPY Sources/Fanex.RemotingRef.NetCore ./Fanex.RemotingRef.NetCore/
WORKDIR /Sources/Fanex.RemotingRef.NetCore
RUN dotnet publish -c release -o /app -r linux-x64 --self-contained false --no-restore

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

# Don’t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
#

# final stage/image
FROM hub.nexdev.net/base/dotnet/core/aspnet:7.0
WORKDIR /app
COPY --from=builder /app ./
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.RemotingRef.NetCore.dll"]