FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WizCo.Orders.sln ./
COPY src/Domain/WizCo.Orders.Domain.csproj src/Domain/
COPY src/Application/WizCo.Orders.Application.csproj src/Application/
COPY src/Infrastructure/WizCo.Orders.Infrastructure.csproj src/Infrastructure/
COPY src/Api/WizCo.Orders.Api.csproj src/Api/
COPY tests/Tests/WizCo.Orders.Tests.csproj tests/Tests/

RUN dotnet restore src/Api/WizCo.Orders.Api.csproj

COPY src/ src/
COPY tests/ tests/

RUN dotnet publish src/Api/WizCo.Orders.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WizCo.Orders.Api.dll"]
