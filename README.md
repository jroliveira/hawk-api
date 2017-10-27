# Hawk (api)

### Pre requirements

* [Visual Studio 2017](https://www.visualstudio.com/vs/whatsnew/)

### Installing

``` bash
$ git clone https://github.com/jroliveira/finance-api.git
```

#### Configuration file (src\Hawk.WebApi\appsettings.json)

``` json
{
  "JwtIssuerOptions": {
    "Issuer": "SuperAwesomeTokenServer",
    "Audience": "http://localhost:48285"
  },
  "Logging": {
    "IncludeScopes": false,
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "Neo4j": {
    "Uri": "bolt://localhost:24786",
    "Username": "neo4j",
    "Password": "neo4j"
  }
}
```

### How to use it

``` bash
F5
```

### Contributions

1. Fork it
2. git checkout -b <branch-name>
3. git add --all && git commit -m "feature description"
4. git push origin <branch-name>
5. Create a pull request
