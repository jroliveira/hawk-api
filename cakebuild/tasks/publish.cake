#load "restore.cake"

Task("Publish")
    .IsDependentOn("Restore")
    .Does(() => DotNetCorePublish("./../src/Hawk.WebApi/", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        OutputDirectory = "./../bin"
    }));
