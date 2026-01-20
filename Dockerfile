# 1. Build mərhələsi
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
# This layer is cached unless you change your NuGet packages
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /out

# 2. Runtime mərhələsi
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the build output from the build stage
COPY --from=build /out .

# Railway environment variable for Port
ENV ASPNETCORE_URLS=http://*:8080
# Note: If Railway provides a ${PORT} variable, it will override this, 
# but "http://*:8080" is a safe default for many cloud providers.

# Match the DLL name to your project name: SellerHub
ENTRYPOINT ["dotnet", "SellerHub.dll"]
