namespace Hawk.WebApi.IntegrationTest.Infrastructure
{
    using System;
    using System.IO;
    using System.Reflection;

    using static System.IO.Path;
    using static System.Uri;

    internal static class ProjectPathFinder
    {
        public static string GetPath(string projectRelativePath, Type startupType)
        {
            var assembly = GetAssembly(startupType);
            var projectName = assembly.GetName().Name;
            var applicationBasePath = GetAssemblyDirectory(assembly);
            var directoryInfo = new DirectoryInfo(applicationBasePath);

            do
            {
                directoryInfo = directoryInfo.Parent;

                if (directoryInfo == null)
                {
                    break;
                }

                var projectDirectoryInfo = new DirectoryInfo(Combine(directoryInfo.FullName, projectRelativePath));
                if (!projectDirectoryInfo.Exists)
                {
                    continue;
                }

                var projectFileInfo = new FileInfo(Combine(projectDirectoryInfo.FullName, projectName, $"{projectName}.csproj"));
                if (projectFileInfo.Exists)
                {
                    return Combine(projectDirectoryInfo.FullName, projectName);
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        }

        private static string? GetAssemblyDirectory(Assembly assembly)
        {
            var codeBase = assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = UnescapeDataString(uri.Path);
            return GetDirectoryName(path);
        }

        private static Assembly GetAssembly(Type? startupType)
        {
            while (startupType?.GetTypeInfo().BaseType != typeof(object))
            {
                startupType = startupType?.BaseType;
            }

            return startupType.GetTypeInfo().Assembly;
        }
    }
}
