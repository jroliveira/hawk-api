Task("Push-Docker-Image")
    .Does<BuildData>(data => DockerPush($"{data.Container.RegistryReference}/{data.Container.ImageReference}"));