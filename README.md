![Hawk - logo][hawk_anime]

# Hawk (api)

[![License: MIT](http://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.txt)

Hawk is a personal finance control. The name Hawk is the name of pig in the anime "The Seven Deadly Sins" (Nanatsu no Taizai).

## Installing / Getting started

This step is to explain what you need to run this application without having to configure the whole dev environment, but if you want to develop for this application, I will explain this step better in the following steps.

### Minimum pre requirements

The minimum pre requirements you need to run this application is the [Docker Compose](https://docs.docker.com/compose/install/).  
After you install the Docker Compose, you need to run the commands below.

### Running application with Docker Compose

``` bash
# Clone this repository
$ git clone https://github.com/jroliveira/hawk-api.git

# Go into the repository
$ cd hawk-api
```

Now you need to configure the [api.env](#configuring-api.env), and run the commands below to run the application.

``` bash
# Run the application
$ docker-compose up
```

## Developing

### Built With

 - [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/)
 - [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
 - [ASP.NET Core](https://docs.microsoft.com/en-ca/aspnet/core/)
 - [Neo4j](https://neo4j.com/developer/)
 - [Docker Compose](https://docs.docker.com/compose/)
 - [Swagger](https://swagger.io/)
 - [Node.js](https://nodejs.org/en/)

### Pre requisites

Download and install:

 - [.NET Core SDK](https://www.microsoft.com/net/download)
 - [Neo4j](https://neo4j.com/download/)
 - [Docker Compose](https://docs.docker.com/compose/install/)
 - [Node.js](https://nodejs.org/en/download/)

### Setting up Dev

``` bash
# Clone this repository
$ git clone https://github.com/jroliveira/hawk-api.git

# Go into the repository
$ cd hawk-api

# Download node packages and install Cake
$ npm install
```

## Configuration

### Configuring appsettings.json

You must create a file `appsettings.json` on the path `./src/Hawk.WebApi/` with the context below.  
This file is used to set the configuration to run in Visual Studio or `dotnet run` command.

``` json
{
  "log": {
    "level": "Info",
    "sinks": {
      "file": {
        "path": "../../logs/.log"
      },
      "elasticSearch": {
        "protocol": "http",
        "host": "elasticsearch",
        "port": 9200
      }
    }
  },
  "authentication": {
    "enabled": true,
    "authority": {
      "protocol": "http",
      "host": "localhost",
      "port": 35653
    }
  },
  "neo4j": {
    "protocol": "bolt",
    "host": "localhost",
    "port": 7687,
    "username": "neo4j",
    "password": "123456"
  },
  "ipRateLimiting": {
    "enableEndpointRateLimiting": false,
    "stackBlockedRequests": false,
    "realIpHeader": "X-Real-IP",
    "clientIdHeader": "X-ClientId",
    "httpStatusCode": 429,
    "ipWhitelist": [],
    "endpointWhitelist": [],
    "clientWhitelist": [],
    "generalRules": [
      {
        "endpoint": "*",
        "period": "1s",
        "limit": 2
      },
      {
        "endpoint": "*",
        "period": "15m",
        "limit": 100
      },
      {
        "endpoint": "*",
        "period": "12h",
        "limit": 1000
      },
      {
        "endpoint": "*",
        "period": "7d",
        "limit": 10000
      }
    ]
  }
}
```

### Configuring api.env

You must create a file `api.env` on the path `.` with the context below.  
This file is used to set the configuration to run in `docker-compose up` command.

``` bash
ASPNETCORE_URLS=http://*:5000

LOG:LEVEL=Info
LOG:SINKS:FILE:PATH=/logs/.log
LOG:SINKS:ELASTICSEARCH:PROTOCOL=http
LOG:SINKS:ELASTICSEARCH:HOST=elasticsearch
LOG:SINKS:ELASTICSEARCH:PORT=9200

AUTHENTICATION:ENABLED=false
AUTHENTICATION:AUTHORITY:PROTOCOL=http
AUTHENTICATION:AUTHORITY:HOST=localhost
AUTHENTICATION:AUTHORITY:PORT=35653

NEO4J:PROTOCOL=bolt
NEO4J:HOST=graphdb
NEO4J:PORT=7687
NEO4J:USERNAME=neo4j
NEO4J:PASSWORD=123456

IPRATELIMITING:ENABLEENDPOINTRATELIMITING=false
IPRATELIMITING:STACKBLOCKEDREQUESTS=false
IPRATELIMITING:REALIPHEADER=X-Real-IP
IPRATELIMITING:CLIENTIDHEADER=X-ClientId
IPRATELIMITING:HTTPSTATUSCODE=429
```

### Building

``` bash
$ dotnet cake
```

### Testing

``` bash
$ dotnet cake --target=Test
```

### Running

If you want to run with Docker Compose, you need to execute the command below:

``` bash
# Run the application
$ docker-compose up
```

or

If you want to run with dotnet, you need to run Neo4j and before, the command below:

``` bash
# Run application
$ dotnet run --project ./src/Hawk.WebApi/
```

### Deploying / Publishing

Manual

Using custom buildpack [dotnetcore-buildpack](https://github.com/jincod/dotnetcore-buildpack)

``` bash
$ heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack
```

or 

Automatic

[![Deploy][heroku_button]][heroku_template]

### Deploying / Publishing

``` bash
$ dotnet cake --target=Release
```

## Api Reference

The documentation was written with [Swagger](https://swagger.io/) and you can see the API documentation on the link [localhost:5001](http://localhost:5001) after performing the previous step [Running](#running) with Docker Compose.

## Licensing

The code is available under the [MIT license](LICENSE.txt).

[hawk_anime]: docs/images/hawk_anime.png "Hawk - logo"
[vs2017]: https://www.visualstudio.com/vs/whatsnew/
[docker_compose]: https://docs.docker.com/compose/
[heroku_button]: https://www.herokucdn.com/deploy/button.svg
[heroku_template]: https://heroku.com/deploy?template=https://github.com/jroliveira/hawk-api
