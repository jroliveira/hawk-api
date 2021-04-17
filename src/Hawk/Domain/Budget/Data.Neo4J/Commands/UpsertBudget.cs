namespace Hawk.Domain.Budget.Data.Neo4J.Commands
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.Budget;
    using Hawk.Domain.Budget.Commands;
    using Hawk.Domain.Shared.Commands;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.IO.Path;
    using static System.String;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class UpsertBudget : Command<UpsertParam<Guid, Budget>>, IUpsertBudget
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Budget", "Data.Neo4J", "Commands", "UpsertBudget.cql"));
        private readonly Neo4JConnection connection;

        public UpsertBudget(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Unit>> Execute(UpsertParam<Guid, Budget> param) => this.connection.ExecuteCypher(
            Statement,
            new
            {
                email = param.Email.Value,
                id = param.Id.ToString(),
                description = param.Entity.Description,
                category = param.Entity.Category.Id,
                year = param.Entity.Recurrence.Start.Year,
                month = param.Entity.Recurrence.Start.Month,
                day = param.Entity.Recurrence.Start.Day,
                frequency = param.Entity.Recurrence.Frequency.ToString(),
                interval = param.Entity.Recurrence.Interval,
                currency = param.Entity.Limit.Currency.Id,
                limit = param.Entity.Limit.Value,
            });
    }
}
