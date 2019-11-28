namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.IO;

    using Hawk.Infrastructure.Monad;

    using static System.IO.File;
    using static System.IO.Path;
    using static System.Reflection.Assembly;

    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class CypherScript
    {
        private static readonly Func<string, Stream> ReadFile = file => OpenRead(Combine(GetDirectoryName(GetExecutingAssembly().Location), file));

        internal static Option<string> ReadCypherScript(string name)
        {
            name = $@"Infrastructure\Data.Neo4J\Entities\{name}";

            if (string.IsNullOrEmpty(name))
            {
                LogError("Cypher script is null or empty.", new { File = name });
                return None();
            }

            try
            {
                using var stream = ReadFile(name);
                if (stream == null)
                {
                    LogError("Variable is null.", new { Var = nameof(stream), Method = nameof(ReadCypherScript), Class = nameof(CypherScript) });
                    return None();
                }

                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
            catch (Exception exception)
            {
                LogError("Cannot load cypher script.", new { File = name, Exception = exception });
                return None();
            }
        }
    }
}
