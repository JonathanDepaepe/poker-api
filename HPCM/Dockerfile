#Build stage

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./WebApi/WebApi.csproj" --disable-parallel
RUN dotnet publish "./WebApi/WebApi.csproj" -c releas -o /app --no-restore

#Serve stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./


EXPOSE 7272

ENTRYPOINT ["dotnet", "WebApi.dll"]
