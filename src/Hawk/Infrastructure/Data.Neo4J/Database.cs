namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.Logging;
    using Hawk.Infrastructure.Monad;

    using Microsoft.Extensions.Options;

    using Neo4j.Driver.V1;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class Database : IDisposable
    {
        private readonly IDriver driver;

        public Database(IOptions<Configuration> config)
        {
            var graphDbConfig = config.Value;
            var auth = AuthTokens.Basic(graphDbConfig.Username, graphDbConfig.Password);

            try
            {
                this.driver = GraphDatabase.Driver(graphDbConfig.Uri, auth);
            }
            catch (Exception exception)
            {
                Logger.Error($"Unable to connect to database {graphDbConfig.Uri}", exception);
            }
        }

        public Task<Try<IEnumerable<TReturn>>> Execute<TReturn>(Func<IRecord, TReturn> mapping, string statement, object parameters) => this.Execute(async session =>
        {
            var cursor = await session.RunAsync(statement, parameters).ConfigureAwait(false);
            var data = await cursor.ToListAsync().ConfigureAwait(false);

            return data.Select(mapping);
        });

        public Task<Try<TReturn>> ExecuteScalar<TReturn>(Func<IRecord, TReturn> mapping, string statement, object parameters) => this.Execute(async session =>
        {
            var cursor = await session.RunAsync(statement, parameters).ConfigureAwait(false);
            var data = await cursor.ToListAsync().ConfigureAwait(false);

            return mapping(data.FirstOrDefault());
        });

        public Task<Try<Unit>> Execute(string statement, object parameters) => this.Execute(async session =>
        {
            await session.RunAsync(statement, parameters).ConfigureAwait(false);
            return Unit();
        });

        public void Dispose() => this.driver.Dispose();

        private async Task<Try<TReturn>> Execute<TReturn>(Func<ISession, Task<TReturn>> command)
        {
            using (var session = this.driver.Session())
            {
                try
                {
                    return await command(session).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    return new Exception("Could not run command in database", exception);
                }
            }
        }
    }
}