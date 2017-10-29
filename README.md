# Hawk (api)

![Hawk - logo][hawk_anime]

Hawk is a personal finance control. The name Hawk is the name of pig in the anime "The Seven Deadly Sins" (Nanatsu no Taizai).

### Pre requirements

* [Visual Studio 2017][vs2017]

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

[hawk_anime]: hawk_anime.png "Hawk - logo"
[vs2017]: https://www.visualstudio.com/vs/whatsnew/
[heroku_button]: https://www.herokucdn.com/deploy/button.svg
[heroku_template]: https://heroku.com/deploy?template=https://github.com/jroliveira/hawk-api