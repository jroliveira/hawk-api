FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR src
COPY . .

RUN dotnet tool restore
RUN dotnet cake --target=Deploy

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

WORKDIR /app
COPY --from=build ./src/artifacts ./

ENTRYPOINT ["dotnet", "Hawk.WebApi.dll"]
