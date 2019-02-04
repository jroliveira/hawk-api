Task("Authenticate-DockerHub")
    .Does<BuildData>(data => DockerLogin(new DockerRegistryLoginSettings
    {
        Username = data.Container.Username,
        Password = data.Container.Password,
    }));