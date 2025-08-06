# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy only StarWarsAPI.csproj and restore
COPY StarWarsAPI.csproj ./
RUN dotnet restore StarWarsAPI.csproj

# Copy the rest of the source
COPY . .

# Publish only the API project
RUN dotnet publish StarWarsAPI.csproj -c Release -o /out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "StarWarsAPI.dll"]
