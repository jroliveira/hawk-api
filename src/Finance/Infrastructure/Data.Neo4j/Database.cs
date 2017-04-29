namespace Finance.Infrastructure.Data.Neo4j
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Options;

    using global::Neo4j.Driver.V1;

    public class Database
    {
        private readonly Config graphDbConfig;

        public Database(IOptions<Config> config)
        {
            this.graphDbConfig = config.Value;
        }

        protected Database()
        {
        }

        public virtual async Task<TResult> ExecuteAsync<TResult>(Func<ISession, Task<TResult>> commandAsync)
        {
            var auth = AuthTokens.Basic(this.graphDbConfig.Username, this.graphDbConfig.Password);

            using (var driver = GraphDatabase.Driver(this.graphDbConfig.Uri, auth))
            using (var session = driver.Session())
            {
                return await commandAsync(session);
            }
        }

        public virtual async Task<ICollection<TResult>> ExecuteAsync<TResult>(Func<IRecord, TResult> mapping, string query, object parameters)
        {
            return await this
                .ExecuteAsync(async session => await Task
                    .Run(() => session
                        .Run(query, parameters)
                        .Select(mapping)
                        .ToList())
                    .ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }
}