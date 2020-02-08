﻿namespace Hawk.Domain.Currency.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class DeleteCurrency : IDeleteCurrency
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Currency", "Data.Neo4J", "DeleteCurrency.cql"));
        private readonly Neo4JConnection connection;

        public DeleteCurrency(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<Unit>> Execute(Option<Email> email, Option<string> name) =>
            email
            && name
            ? this.connection.ExecuteCypher(
                Statement,
                new
                {
                    email = email.Get().Value,
                    name = name.Get(),
                })
            : Task(Failure<Unit>(new NullObjectException("Parameters are required.")));
    }
}
