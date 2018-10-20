Task("Publish")
    .Does(() => DotNetCorePublish(
		"./../src/Hawk.WebApi/", 
		new DotNetCorePublishSettings
		{
			Configuration = configuration,
			OutputDirectory = outputDirectory,
			NoBuild = true,
            ArgumentCustomization = args => args.Append($"--no-restore"),
		}));