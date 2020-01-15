namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.IO;

    using Hawk.Infrastructure.Monad;

    using static System.IO.File;
    using static System.IO.Path;
    using static System.Reflection.Assembly;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;
    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class CypherScript
    {
        private static readonly Func<string, Stream?> ReadFile = file =>
        {
            var directoryName = GetDirectoryName(GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                return OpenRead(Combine(directoryName, file));
            }

            LogError("Directory name is null.", new { Path = GetExecutingAssembly().Location });
            return default;
        };

        internal static Option<string> ReadCypherScript(string name)
        {
            name = Combine("Domain", name);

            if (string.IsNullOrEmpty(name))
            {
                LogError("Cypher script is default or empty.", new { File = name });
                return None();
            }

            try
            {
                using var stream = ReadFile(name);
                if (stream == default)
                {
                    LogError("Cypher stream is default.", new { File = name });
                    return None();
                }

                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                LogError("Cannot load cypher script.", new { File = name }, HandleException(exception));
                return None();
            }
        }
    }
}
