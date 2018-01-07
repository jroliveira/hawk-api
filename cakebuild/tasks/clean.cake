Task("Clean")
    .Does(() => CleanDirectory(Directory("./../bin")));