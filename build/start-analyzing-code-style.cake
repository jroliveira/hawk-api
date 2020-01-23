Task("Start-Analyze-Code-Style")
    .Does<BuildData>(data =>
    {
        StartProcess("dotnet.exe", "tool install --global dotnet-sonarscanner --version 4.8.0");

        using (var process = StartAndReturnProcess("dotnet", new ProcessSettings()
			.SetRedirectStandardOutput(true)
			.WithArguments(arguments =>
			{
				arguments.Append("sonarscanner");
				arguments.Append("begin");
				arguments.Append($"/k:{data.Analyze.Key}");
                arguments.Append($"/d:sonar.host.url={data.Analyze.Host}");
            })))
            {
		        process.WaitForExit();

                if (process.GetExitCode() != 0)
		        {
			        throw new CakeException("Could not start SonarQube analysis");
		        }
	        }
    });
