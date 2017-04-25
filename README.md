# Finance (api)

### Pre requirements

* [.NET Core SDK (contains .NET Core 1.0 and 1.1)](https://www.microsoft.com/net/download/core#/sdk)
 * Windows (x64) Installer
 * Visual Studio 2015 Tools
 * Visual Studio 2017 Tools

### Installing

``` bash
$ git clone https://github.com/jroliveira/finance-api.git
```

### Building

``` bash
$ dotnet restore
```

### How to use it

``` bash
$ dotnet run --project src/Finance.WebApi/Finance.WebApi.csproj
```

### Running tests

- Data.Query

``` bash 
$ dotnet test test/Finance/Finance.Tests.csproj
```

### Running in Docker (win7)

#### Pre requirements

* [Docker Toolbox](https://docs.docker.com/toolbox/toolbox_install_windows/)

#### How to use it

``` bash
$ docker-compose up
```

### Contributions

1. Fork it
2. git checkout -b <branch-name>
3. git add --all && git commit -m "feature description"
4. git push origin <branch-name>
5. Create a pull request
