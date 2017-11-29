namespace Hawk.Infrastructure.Data.Neo4j
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using global::Neo4j.Driver.V1;

    public class Database : IDisposable
    {
        private readonly IDriver driver;

        public Database(IOptions<Neo4JConfig> config)
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

        protected Database()
        {
        }

        public virtual async Task<TResult> Execute<TResult>(Func<ISession, Task<TResult>> command)
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

        public virtual async Task<ICollection<TResult>> Execute<TResult>(Func<IRecord, TResult> mapping, string query, object parameters)
        {
            return await this.Execute(async session =>
            {
                var cursor = await session.RunAsync(query, parameters).ConfigureAwait(false);
                var data = await cursor.ToListAsync().ConfigureAwait(false);

                return data.Select(mapping).ToList();
            });
        }

        public void Dispose()
        {
            this.driver.Dispose();
        }
    }
}