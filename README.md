# Hawk (api)

Hawk is a personal finance control. The name Hawk is the name of pig in the anime "The Seven Deadly Sins" (Nanatsu no Taizai).

### Pre requirements

* [Visual Studio 2017](https://www.visualstudio.com/vs/whatsnew/)

### Installing

``` bash
$ git clone https://github.com/jroliveira/hawk-api.git
```

#### Configuration file (src\Hawk.WebApi\appsettings.json)

``` json
{
  "jwtIssuerOptions": {
    "issuer": "SuperAwesomeTokenServer",
    "audience": "http://localhost:48285"
  },
  "logging": {
    "includeScopes": false,
    "logLevel": {
      "default": "Debug",
      "system": "Information",
      "microsoft": "Information"
    }
  },
  "neo4j": {
    "uri": "bolt://localhost:7687",
    "username": "neo4j",
    "password": "neo4j"
  },
  "graphql": {
    "enabled": false,
    "path": "/graphql",
    "managerPath": "/graphiql"
  }
}
```

### How to use it

``` bash
F5
```

### Deploy to Heroku

#### Manual

Using custom buildpack [dotnetcore-buildpack](https://github.com/jincod/dotnetcore-buildpack)

``` bash
$ heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack
```

#### Automatic

[![Deploy](https://www.herokucdn.com/deploy/button.svg)](https://heroku.com/deploy?template=https://github.com/jroliveira/hawk-api)

### Contributions

1. Fork it
2. git checkout -b <branch-name>
3. git add --all && git commit -m "feature description"
4. git push origin <branch-name>
5. Create a pull request
