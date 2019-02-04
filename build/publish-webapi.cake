Task("Publish-WebApi")
    .Does<BuildData>(data => DotNetCorePublish(data.Solution.WebApiPath, new DotNetCorePublishSettings
    {
        Configuration = data.Configuration,
        NoBuild = true,
        NoRestore = true,
        OutputDirectory = data.Solution.ArtifactsDirectory,
        MSBuildSettings = new DotNetCoreMSBuildSettings
        {
            NoLogo = true,
        },
    }));