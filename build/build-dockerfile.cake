Task("Build-Dockerfile")
    .Does<BuildData>(data => DockerBuild(new DockerImageBuildSettings
    {
        Quiet = true,
        ForceRm = true,
        File = data.Container.FilePath,
        Tag = new[] { $"{data.Container.RegistryReference}/{data.Container.ImageReference}" },
    },
    "./"));