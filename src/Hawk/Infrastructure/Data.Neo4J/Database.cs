namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.Options;

    using Neo4j.Driver.V1;

    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Neo4j.Driver.V1.AuthTokens;
    using static Neo4j.Driver.V1.GraphDatabase;

    internal sealed class Database : IDisposable
    {
        private readonly IDriver driver;

        public Database(IOptions<Configuration> configOptional)
        {
            var config = configOptional.Value;
            var auth = Basic(config.Username, config.Password);
            var uri = $"{config.Protocol}://{config.Host}:{config.Port}";

            try
            {
                this.driver = Driver(uri, auth);
            }
            catch (Exception exception)
            {
                Error($"Unable to connect to database {uri}", exception);
            }
        }

        public void Dispose() => this.driver.Dispose();

        internal Task<Try<IEnumerable<TReturn>>> Execute<TReturn>(Func<IRecord, TReturn> mapping, string statement, object parameters) => this.Execute(async session =>
        {
            var cursor = await session.RunAsync(statement, parameters).ConfigureAwait(false);
            var data = await cursor.ToListAsync().ConfigureAwait(false);

            return data.Select(mapping);
        });

        internal Task<Try<TReturn>> ExecuteScalar<TReturn>(Func<IRecord, TReturn> mapping, string statement, object parameters) => this.Execute(async session =>
        {
            var cursor = await session.RunAsync(statement, parameters).ConfigureAwait(false);
            var data = await cursor.ToListAsync().ConfigureAwait(false);

            return mapping(data.FirstOrDefault());
        });

        internal Task<Try<Unit>> Execute(string statement, object parameters) => this.Execute(async session =>
        {
            await session.RunAsync(statement, parameters).ConfigureAwait(false);
            return Unit();
        });

        private async Task<Try<TReturn>> Execute<TReturn>(Func<ISession, Task<TReturn>> command)
        {
            try
            {
                using (var session = this.driver.Session())
                {
                    return await command(session).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                return new Exception("Could not run command in database", exception);
            }
        }
    }
}
