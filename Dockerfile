FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy the solution file and project files
COPY SafaFoods.sln ./
COPY backend/src/SafaFoods.Api/SafaFoods.Api.csproj backend/src/SafaFoods.Api/
COPY backend/src/SafaFoods.Core/SafaFoods.Core.csproj backend/src/SafaFoods.Core/
COPY backend/src/SafaFoods.Infrastructure/SafaFoods.Infrastructure.csproj backend/src/SafaFoods.Infrastructure/
COPY backend/tests/SafaFoods.Api.Tests/SafaFoods.Api.Tests.csproj backend/tests/SafaFoods.Api.Tests/

# Restore dependencies
RUN dotnet restore

# Copy the remaining source code
COPY backend/ backend/

# Build and publish the application
WORKDIR /app/backend/src/SafaFoods.Api
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/out .

# Use the PORT environment variable provided by Render, defaulting to 8080
CMD ["sh", "-c", "dotnet SafaFoods.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
