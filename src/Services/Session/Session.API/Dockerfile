FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster
ARG BUILD_CONFIGURATION=Debug
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
EXPOSE 80

WORKDIR /src

COPY ["Services/Session/Session.API/Session.API.csproj", "Services/Session/Session.API/"]

RUN dotnet restore Services/Session/Session.API/Session.API.csproj
COPY . .
WORKDIR "Services/Session/Session.API/"

RUN dotnet build -c $BUILD_CONFIGURATION

ENTRYPOINT ["dotnet", "run"]