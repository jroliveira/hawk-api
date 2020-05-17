FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR src
COPY . .

RUN dotnet tool restore
RUN dotnet cake --target=Deploy

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

WORKDIR /app
COPY --from=build ./src/artifacts ./

ENTRYPOINT ["dotnet", "Hawk.WebApi.dll"]
