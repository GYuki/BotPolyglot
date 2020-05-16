FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
EXPOSE 80

WORKDIR /src

COPY ["Services/Receiver/Receiver.API/Receiver.csproj", "Services/Receiver/Receiver.API/"]

RUN dotnet restore Services/Receiver/Receiver.API/Receiver.csproj
COPY . .
WORKDIR "Services/Receiver/Receiver.API/"

RUN dotnet build -c $BUILD_CONFIGURATION

ENTRYPOINT ["dotnet", "run"]