FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
EXPOSE 80

WORKDIR /src



COPY ["ApiGateways/Telegram.Sender/Telegram.Sender.csproj", "ApiGateways/Telegram.Sender/"]

RUN dotnet restore ApiGateways/Telegram.Sender/Telegram.Sender.csproj
COPY . .
WORKDIR "ApiGateways/Telegram.Sender/"

RUN dotnet build -c $BUILD_CONFIGURATION

ENTRYPOINT ["dotnet", "run"]