#load "tasks/build.cake"
#load "tasks/clean.cake"
#load "tasks/publish.cake"
#load "tasks/restore.cake"
#load "tasks/test.cake"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var solutionPath = $"./../Hawk.sln";
var outputDirectory = Directory($"./../bin");

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
	.IsDependentOn("Publish");

RunTarget(target);