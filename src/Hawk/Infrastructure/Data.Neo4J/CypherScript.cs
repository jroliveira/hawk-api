namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.IO;
    using System.Reflection;

    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Logging.Logger;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class CypherScript
    {
        private static readonly Func<string, Stream> ReadFile = typeof(CypherScript).GetTypeInfo().Assembly.GetManifestResourceStream;

        internal static Option<string> ReadCypherScript(string name)
        {
            name = $@"Hawk.Infrastructure.Data.Neo4J.Entities.{name}";

            if (string.IsNullOrEmpty(name))
            {
                LogError($"File {name} is null or empty.");
                return None();
            }

            try
            {
                using (var stream = ReadFile(name))
                {
                    if (stream == null)
                    {
                        LogError($"Variable {nameof(stream)} of the class {nameof(CypherScript)} in the method {nameof(ReadCypherScript)} is null.");
                        return None();
                    }

                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception exception)
            {
                LogError($"Cannot load file {name}.", exception);
                return None();
            }
        }
    }
}
