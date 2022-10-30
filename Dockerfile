FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY ./ ./
RUN dotnet restore

RUN dotnet publish example_site/ExampleSite.csproj -c Release -o /app

# 2nd stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "ExampleSite.dll"]
EXPOSE 80/tcp
EXPOSE 443/tcp