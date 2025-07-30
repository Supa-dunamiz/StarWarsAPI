# Stage 1: Build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy sln and project files
COPY StarWarsAPI.sln .
COPY StarWarsAPI/StarWarsAPI.csproj StarWarsAPI/

# Restore only what’s necessary
RUN dotnet restore

# Copy everything else
COPY . .

# Build and publish only the API project
WORKDIR /src/StarWarsAPI
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Serve the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "StarWarsAPI.dll"]
