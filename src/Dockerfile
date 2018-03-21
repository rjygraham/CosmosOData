FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY ./CosmosOData.sln ./
COPY CosmosOData.Api/CosmosOData.Api.csproj CosmosOData.Api/
COPY CosmosOData.Models/CosmosOData.Models.csproj CosmosOData.Models/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/CosmosOData.Api
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CosmosOData.Api.dll"]
