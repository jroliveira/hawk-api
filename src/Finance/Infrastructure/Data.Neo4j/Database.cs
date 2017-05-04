namespace Finance.Infrastructure.Data.Neo4j
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        public virtual TResult Execute<TResult>(Func<ISession, TResult> commandAsync)
        {
            var auth = AuthTokens.Basic(this.graphDbConfig.Username, this.graphDbConfig.Password);

            using (var driver = GraphDatabase.Driver(this.graphDbConfig.Uri, auth))
            using (var session = driver.Session())
            {
                return commandAsync(session);
            }
        }

        public virtual ICollection<TResult> Execute<TResult>(Func<IRecord, TResult> mapping, string query, object parameters)
        {
            return this.Execute(session => session
                .Run(query, parameters)
                .Select(mapping)
                .ToList());
        }
    }
}