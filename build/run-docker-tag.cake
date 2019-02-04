Task("Run-Docker-Tag")
    .Does<BuildData>(data => DockerTag(data.Container.RegistryReference, data.Container.ImageReference));