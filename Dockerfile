FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /source

COPY . .
RUN dotnet restore
RUN dotnet publish --output /app/ --configuration Release

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "Hawk.WebApi.dll"]