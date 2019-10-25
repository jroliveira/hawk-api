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

        public Database(IOptions<Configuration> config)
        {
            var auth = Basic(config.Value.Username, config.Value.Password);
            var uri = $"{config.Value.Protocol}://{config.Value.Host}:{config.Value.Port}";

            try
            {
                this.driver = Driver(uri, auth);
            }
            catch (Exception exception)
            {
                LogError($"Unable to connect to database {uri}", exception);
            }
        }

        public void Dispose() => this.driver?.Dispose();

        internal Task<Try<TReturn>> ExecuteScalar<TReturn>(Func<IRecord, Try<TReturn>> mapping, Option<string> statement, object parameters) => this.ExecuteAndGetRecords(
            statement,
            parameters,
            records => records.Count > 1
                ? new Exception($"Query returned {records.Count} results.")
                : mapping(records.FirstOrDefault()));

        internal Task<Try<IEnumerable<Try<TReturn>>>> Execute<TReturn>(Func<IRecord, Try<TReturn>> mapping, Option<string> statement, object parameters) => this.ExecuteAndGetRecords(
            statement,
            parameters,
            records => Success(records.Select(mapping)));

        internal Task<Try<Unit>> Execute(Option<string> statement, object parameters) => this.ExecuteAndGetRecords(
            statement,
            parameters,
            _ => Success(Unit()));

        private Task<Try<TReturn>> ExecuteAndGetRecords<TReturn>(Option<string> statement, object parameters, Func<IList<IRecord>, Try<TReturn>> command) => this.ExecuteAndGetCursor(
            statement,
            parameters,
            async cursor =>
            {
                var records = await cursor.ToListAsync().ConfigureAwait(false);

                return command(records);
            });

        private async Task<Try<TReturn>> ExecuteAndGetCursor<TReturn>(Option<string> statement, object parameters, Func<IStatementResultCursor, Task<Try<TReturn>>> command)
        {
            if (!statement.IsDefined)
            {
                return new ArgumentNullException(nameof(statement), "Cypher statement is required.");
            }

            if (this.driver == null)
            {
                return new NullReferenceException("Neo4j driver is null.");
            }

            try
            {
                using (var session = this.driver.Session())
                {
                    var cursor = await session.RunAsync(statement.Get(), parameters).ConfigureAwait(false);

                    return await command(cursor).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                return new Exception($"Could not run command in database {exception.Message}", exception);
            }
        }
    }
}
