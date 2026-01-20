# 1. Build mərhələsi
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file from the SellerHub folder
COPY ["SellerHub/SellerHub.csproj", "SellerHub/"]

# Restore based on that specific path
RUN dotnet restore "SellerHub/SellerHub.csproj"

# Copy the rest of the files
COPY . .

# Set working directory to the project folder and publish
WORKDIR "/src/SellerHub"
RUN dotnet publish "SellerHub.csproj" -c Release -o /app/publish

# 2. Runtime mərhələsi
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "SellerHub.dll"]
