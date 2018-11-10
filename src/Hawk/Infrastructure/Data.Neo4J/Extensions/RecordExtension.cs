namespace Hawk.Infrastructure.Data.Neo4J.Extensions
{
    using System.Linq;

    using Hawk.Infrastructure.Monad;

    using Neo4j.Driver.V1;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal static class RecordExtension
    {
        internal static Option<Record> GetRecord(this IRecord @this, string key)
        {
            if (@this == null || !@this.Keys.Contains(key))
            {
                return None;
            }

            return new Record(@this[key]);
        }
    }
}
