FROM microsoft/dotnet:1.1.1-sdk

COPY . /app
WORKDIR /app

RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--project", "src/Hawk.WebApi/Hawk.WebApi.csproj"]
