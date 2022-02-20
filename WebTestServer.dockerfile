# docker build -t test-web-server -f WebTestServer.dockerfile .

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build-env
WORKDIR /app

COPY Handler/ ./Handler
COPY WebTestServer/ ./WebTestServer
RUN dotnet publish -c Release -r linux-x64 --self-contained -o out WebTestServer

FROM ubuntu:18.04
RUN apt-get update -qq && apt-get install -y -qq libssl-dev libgdiplus
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 5000/tcp

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENTRYPOINT ["./WebTestServer"]
