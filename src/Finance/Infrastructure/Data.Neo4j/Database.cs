namespace Finance.Infrastructure.Data.Neo4j
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.Options;

    using global::Neo4j.Driver.V1;

    public class Database : IDisposable
    {
        private readonly Config graphDbConfig;
        private readonly IDriver driver;

        public Database(IOptions<Config> config)
        {
            this.graphDbConfig = config.Value;
            var auth = AuthTokens.Basic(this.graphDbConfig.Username, this.graphDbConfig.Password);

            try
            {
                this.driver = GraphDatabase.Driver(this.graphDbConfig.Uri, auth);
            }
            catch (Exception exception)
            {
                throw new Exception($"Unable to connect to database {this.graphDbConfig.Uri}", exception);
            }
        }

        protected Database()
        {
        }

        public virtual TResult Execute<TResult>(Func<ISession, TResult> command)
        {
            using (var session = this.driver.Session())
            {
                try
                {
                    return command(session);
                }
                catch (Exception exception)
                {
                    throw new Exception("Could not run command in database", exception);
                }
            }
        }

        public virtual ICollection<TResult> Execute<TResult>(Func<IRecord, TResult> mapping, string query, object parameters)
        {
            return this.Execute(session => session
                .Run(query, parameters)
                .Select(mapping)
                .ToList());
        }

        public void Dispose()
        {
            this.driver.Dispose();
        }
    }
}