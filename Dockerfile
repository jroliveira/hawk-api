FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build

# install cakebuild 0.35.0
RUN dotnet tool install --global Cake.Tool --version 0.35.0
ENV PATH="${PATH}:/root/.dotnet/tools"

ADD . /src

RUN dotnet-cake /src/build.cake --target=Deploy

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime

WORKDIR /app

COPY --from=build ./src/artifacts ./
ENTRYPOINT ["dotnet", "Hawk.WebApi.dll"]
