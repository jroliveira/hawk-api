Task("Stop-Analyze-Code-Style")
    .Does<BuildData>(data => StartProcess(
        "dotnet.exe",
        "sonarscanner end"));
