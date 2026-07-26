FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY SafaFoods.sln .
COPY src/SafaFoods.Api/SafaFoods.Api.csproj src/SafaFoods.Api/
COPY src/SafaFoods.Core/SafaFoods.Core.csproj src/SafaFoods.Core/
COPY src/SafaFoods.Infrastructure/SafaFoods.Infrastructure.csproj src/SafaFoods.Infrastructure/
COPY tests/SafaFoods.Api.Tests/SafaFoods.Api.Tests.csproj tests/SafaFoods.Api.Tests/
RUN dotnet restore
COPY . .
WORKDIR /app/src/SafaFoods.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/SafaFoods.Api/out ./
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SafaFoods.Api.dll"]
