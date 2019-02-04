Task("Setup-Tests")
    .Does(() => DockerComposeUp(new DockerComposeUpSettings
    {
        Files = new [] { "docker-compose-integration.yml" },
        AbortOnContainerExit = true,
        Build = true,
        ForceRecreate = true,
    }));