# 1. Build mərhələsi
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# csproj faylını kopyala
COPY *.csproj ./

# NuGet paketlərini yüklə
RUN dotnet restore

# Layihə fayllarını kopyala
COPY . ./

# Release olaraq publish et
RUN dotnet publish -c Release -o /out

# 2. Runtime mərhələsi
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Build nəticəsini runtime-a köçür
COPY --from=build /out .

# Railway portu istifadə edir
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

# DLL adını dəyiş (öz project adınla)
ENTRYPOINT ["dotnet", "Martrio.Backend.dll"]
