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
        private static readonly Assembly Assembly = typeof(CypherScript).GetTypeInfo().Assembly;

        internal static Option<string> ReadAll(string name)
        {
            name = $@"Hawk.Infrastructure.Data.Neo4J.Entities.{name}";

            if (string.IsNullOrEmpty(name))
            {
                LogError("File name cannot be null or empty.");
                return None;
            }

            try
            {
                using (var stream = Assembly.GetManifestResourceStream(name))
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception exception)
            {
                LogError($"Cannot load file {name}.", exception);
                return None;
            }
        }
    }
}
