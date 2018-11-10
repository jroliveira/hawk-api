Task("Publish")
    .Does(() => DotNetCorePublish(
        "./../src/Hawk.WebApi/", 
        new DotNetCorePublishSettings
        {
            Configuration = configuration,
            NoBuild = true,
            NoRestore = true,
            OutputDirectory = artifactsDirectory,
            MSBuildSettings = new DotNetCoreMSBuildSettings
            {
                NoLogo = true,    
            },
        }));