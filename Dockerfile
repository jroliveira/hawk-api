FROM microsoft/aspnetcore-build:1.1 AS builder
WORKDIR /source

COPY . .
RUN dotnet restore
RUN dotnet publish --output /app/ --configuration Release

FROM microsoft/aspnetcore:1.1
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "Hawk.WebApi.dll"]