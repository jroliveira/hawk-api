#load "clean.cake"

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => DotNetCoreRestore("./../Hawk.sln"));