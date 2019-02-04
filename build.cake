#addin "Cake.Docker&version=0.9.7"
#addin "Cake.Figlet&version=1.2.0"

#load "build/*.cake"

Setup<BuildData>(context =>
{
    Information(Figlet("Hawk"));

    return new BuildData(
        context,
        GetConfiguration(),
        ErrorHandler,
        new RepositoryData(
            "origin"),
        new SolutionData(
            "./artifacts",
            "./Hawk.sln",
            "./src/Hawk.WebApi/"),
        new ContainerData(
            "docker-compose-integration.yml",
            EnvironmentVariable("DOCKERHUB_USERNAME"),
            EnvironmentVariable("DOCKERHUB_PASSWORD"),
            "junroliveira",
            "hawk-api-test"));
});

Task("Default")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build-Solution");

Task("Test")
    .IsDependentOn("Setup-Tests");

Task("Deploy")
    .IsDependentOn("Delete-Temp-Directories")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build-Solution")
    .IsDependentOn("Publish-WebApi");

Task("Release")
    .IsDependentOn("Setup-Tests")
    .IsDependentOn("Build-Dockerfile")
    .IsDependentOn("Authenticate-DockerHub")
    .IsDependentOn("Run-Docker-Tag")
    .IsDependentOn("Push-Docker-Image");

RunTarget(Argument("target", "Default"));
