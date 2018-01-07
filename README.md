# Hawk (api)

[![CircleCI](https://circleci.com/gh/jroliveira/hawk-api/tree/master.svg?style=svg)](https://circleci.com/gh/jroliveira/hawk-api/tree/master)
[![CodeFactor](https://www.codefactor.io/repository/github/jroliveira/hawk-api/badge)](https://www.codefactor.io/repository/github/jroliveira/hawk-api)

![Hawk - logo][hawk_anime]

Hawk is a personal finance control. The name Hawk is the name of pig in the anime "The Seven Deadly Sins" (Nanatsu no Taizai).

### Installing

``` bash
$ git clone https://github.com/jroliveira/hawk-api.git
```

### Running in Visual Studio 2017

#### Pre requirements

* [Visual Studio 2017][vs2017]

#### Configuration file (.\src\Hawk.WebApi\appsettings.json)

``` json
{
  "authentication": {
    "authority": "http://localhost"
  },
  "neo4j": {
    "uri": "bolt://localhost:7687",
    "username": "neo4j",
    "password": "neo4j"
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

### How to use it

``` bash
F5
```

### Running in Docker

#### Pre requirements

* [Docker Compose][docker_compose]

#### Configuration file (.\api.env)

``` bash
ASPNETCORE_URLS=http://*:5000

NEO4J:URI=bolt://graphdb:7687
NEO4J:USERNAME=neo4j
NEO4J:PASSWORD=123456

AUTHENTICATION:AUTHORITY=http://localhost:35653

IPRATELIMITING:ENABLEENDPOINTRATELIMITING=false
IPRATELIMITING:STACKBLOCKEDREQUESTS=false
IPRATELIMITING:REALIPHEADER=X-Real-IP
IPRATELIMITING:CLIENTIDHEADER=X-ClientId
IPRATELIMITING:HTTPSTATUSCODE=429
```

### How to use it

``` bash
$ docker-compose up
```

### Deploy to Heroku

#### Manual

Using custom buildpack [dotnetcore-buildpack]()

``` bash
$ heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack
```

#### Automatic

[![Deploy][heroku_button]][heroku_template]

### Contributions

1. Fork it
2. git checkout -b <branch-name>
3. git add --all && git commit -m "feature description"
4. git push origin <branch-name>
5. Create a pull request

### License

The code is available under the [MIT license](LICENSE).

[hawk_anime]: docs/images/hawk_anime.png "Hawk - logo"
[vs2017]: https://www.visualstudio.com/vs/whatsnew/
[docker_compose]: https://docs.docker.com/compose/
[heroku_button]: https://www.herokucdn.com/deploy/button.svg
[heroku_template]: https://heroku.com/deploy?template=https://github.com/jroliveira/hawk-api