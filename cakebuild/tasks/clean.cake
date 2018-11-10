Task("Clean")
    .Does(() => {
        CleanDirectory(artifactsDirectory);
        Information($"  Clean completed for directory \"{artifactsDirectory}\".");
    });