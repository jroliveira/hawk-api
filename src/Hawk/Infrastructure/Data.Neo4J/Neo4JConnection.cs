namespace Hawk.Infrastructure.Data.Neo4J
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Resilience;

    using Microsoft.Extensions.Options;

    using Neo4j.Driver;

    using static Hawk.Infrastructure.ErrorHandling.ExceptionHandler;
    using static Hawk.Infrastructure.Logging.Logger;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    using static Neo4j.Driver.AuthTokens;
    using static Neo4j.Driver.GraphDatabase;

    using Unit = Hawk.Infrastructure.Monad.Unit;

    internal sealed class Neo4JConnection : IDisposable
    {
        private readonly ResiliencePolicy resiliencePolicy;
        private readonly IDriver? driver;

        public Neo4JConnection(IOptions<Neo4JConfiguration> config, ResiliencePolicy resiliencePolicy)
        {
            this.resiliencePolicy = resiliencePolicy;
            var auth = Basic(config.Value.Username, config.Value.Password);

            try
            {
                this.driver = Driver(config.Value.Uri, auth);
            }
            catch (Exception exception)
            {
                LogError("Unable to connect to Neo4j.", new { config.Value.Uri }, HandleException(exception));
            }
        }

        public void Dispose() => this.driver?.Dispose();

        internal Task<Try<TReturn>> ExecuteCypherScalar<TReturn>(
            Func<IRecord, Try<TReturn>> mapping,
            Option<string> statement,
            object parameters) => this.ExecuteCypherAndGetRecords(
                statement,
                parameters,
                records => records.Count > 1
                    ? new InternalException($"Query returned {records.Count} results.")
                    : mapping(records.FirstOrDefault()));

        internal Task<Try<IEnumerable<Try<TReturn>>>> ExecuteCypher<TReturn>(
            Func<IRecord, Try<TReturn>> mapping,
            Option<string> statement,
            object parameters) => this.ExecuteCypherAndGetRecords(
                statement,
                parameters,
                records => Success(records.Select(mapping)));

        internal Task<Try<Unit>> ExecuteCypher(
            Option<string> statement,
            object parameters) => this.ExecuteCypherAndGetRecords(
                statement,
                parameters,
                _ => Success(Unit()));

        private Task<Try<TReturn>> ExecuteCypherAndGetRecords<TReturn>(
            Option<string> statement,
            object parameters,
            Func<IList<IRecord>, Try<TReturn>> command) => this.ExecuteCypherAndGetCursor(
                statement,
                parameters,
                async cursor => command(await cursor.ToListAsync()));

        private async Task<Try<TReturn>> ExecuteCypherAndGetCursor<TReturn>(
            Option<string> statement,
            object parameters,
            Func<IResultCursor, Task<Try<TReturn>>> command)
        {
            if (!statement.IsDefined)
            {
                return new NullParameterException(nameof(statement), "Cypher statement is required.");
            }

            if (this.driver == default)
            {
                return new NullObjectException("Neo4j driver is default.");
            }

            try
            {
                return await this.resiliencePolicy.Execute(async _ =>
                {
                    var session = this.driver.AsyncSession(o => o.WithDatabase("neo4j"));
                    var cursor = await session.RunAsync(statement.Get(), parameters);

                    return await command(cursor);
                });
            }
            catch (Exception exception)
            {
                return new InternalException("Could not run command in Neo4j.", exception);
            }
        }
    }
}
