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

If you want to run with dotnet, you need to run the command below:

``` bash
# Run dependencies
$ dotnet cake --target=Dependencies

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
