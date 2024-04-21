FROM mcr.microsoft.comdotnetaspnet8.0 AS base
WORKDIR app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.comdotnetsdk8.0 AS build
WORKDIR src
COPY ["BirdBird.csproj", "Bird"]
COPY ["InfrastructureInfrastructure.csproj", "Infrastructure"]
COPY ["ApplicationApplication.csproj", "Application"]
COPY ["DomainDomain.csproj", "Domain"]
RUN dotnet restore BirdBird.csproj
COPY . .
WORKDIR srcBird
RUN dotnet build Bird.csproj -c Release -o appbuild

FROM build AS publish
RUN dotnet publish Bird.csproj -c Release -o apppublish pUseAppHost=false

FROM base AS final
WORKDIR app
COPY --from=publish apppublish .
ENTRYPOINT ["dotnet","Bird.dll"]
