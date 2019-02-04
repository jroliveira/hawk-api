FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder

# install cakebuild 0.33.0
RUN dotnet tool install --global Cake.Tool --version 0.33.0
ENV PATH="${PATH}:/root/.dotnet/tools"

ADD . /src

RUN dotnet-cake /src/build.cake --Target=Deploy

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /app

COPY --from=builder ./src/artifacts .
ENTRYPOINT ["dotnet", "Hawk.WebApi.dll"]
