public sealed class BuildData
{
    public BuildData(
        ICakeContext context,
        string configuration,
        Func<BuildData, Action<Exception>> errorHandler,
        RepositoryData repository,
        SolutionData solution,
        ContainerData container,
        AnalyzeData analyze)
    {
        this.Configuration = configuration;
        this.Repository = repository;
        this.Solution = solution;
        this.Container = container;
        this.Analyze = analyze;

        this.DirectoriesToDelete = context
            .GetDirectories(this.Solution.ArtifactsDirectory)
            .Concat(context.GetDirectories("./src/**/bin"))
            .Concat(context.GetDirectories("./src/**/obj"))
            .OrderBy(directory => directory.ToString())
            .ToList();

        this.ErrorHandler = errorHandler(this);
    }

    public string Configuration { get; }
    public Action<Exception> ErrorHandler { get; }
    public RepositoryData Repository { get; }
    public SolutionData Solution { get; }
    public ContainerData Container { get; }
    public AnalyzeData Analyze { get; }
    public IEnumerable<DirectoryPath> DirectoriesToDelete { get; }
}

public sealed class RepositoryData
{
    public RepositoryData(string remote) => this.Remote = remote;

    public string Remote { get; }
}

public sealed class SolutionData
{
    public SolutionData(
        string artifactsDirectory,
        string slnPath,
        string webApiPath)
    {
        this.ArtifactsDirectory = artifactsDirectory;
        this.SlnPath = slnPath;
        this.WebApiPath = webApiPath;
    }

    public string ArtifactsDirectory { get; }
    public string SlnPath { get; }
    public string WebApiPath { get; }
}

public sealed class ContainerData
{
    public ContainerData(
        string filePath,
        string username,
        string password,
        string registryReference,
        string imageReference)
    {
        this.FilePath = filePath;
        this.Username = username;
        this.Password = password;
        this.RegistryReference = registryReference;
        this.ImageReference = imageReference;
    }

    public string FilePath { get; }
    public string Username { get; }
    public string Password { get; }
    public string RegistryReference { get; }
    public string ImageReference { get; }
}

public sealed class AnalyzeData
{
    public AnalyzeData(
        string host,
        string key)
    {
        this.Host = host;
        this.Key = key;
    }

    public string Host { get; }

    public string Key { get; }
}
