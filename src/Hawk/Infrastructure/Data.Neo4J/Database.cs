namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using Neo4j.Driver.V1;

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
                throw new Exception($"Unable to connect to database {graphDbConfig.Uri}", exception);
            }
        }

        public async Task<IEnumerable<TReturn>> Execute<TReturn>(Func<IRecord, TReturn> mapping, string statement, object parameters)
        {
            return await this.Execute(async session =>
            {
                var cursor = await session.RunAsync(statement, parameters).ConfigureAwait(false);
                var data = await cursor.ToListAsync().ConfigureAwait(false);

                return data.Select(mapping);
            });
        }

        public async Task Execute(string statement, object parameters)
        {
            await this.Execute(async session => await session.RunAsync(statement, parameters).ConfigureAwait(false));
        }

        public void Dispose()
        {
            this.driver.Dispose();
        }

        private async Task<TResult> Execute<TResult>(Func<ISession, Task<TResult>> command)
        {
            using (var session = this.driver.Session())
            {
                try
                {
                    return await command(session).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not run command in database", exception);
                }
            }
        }
    }
}