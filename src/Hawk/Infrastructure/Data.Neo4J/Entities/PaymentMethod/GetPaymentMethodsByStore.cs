﻿namespace Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Data.Neo4J.Entities.PaymentMethod.PaymentMethodMapping;

    using static System.String;

    internal sealed class GetPaymentMethodsByStore : IGetPaymentMethodsByStore
    {
        private static readonly Option<string> Statement = ReadAll("PaymentMethod.GetPaymentMethodsByStore.cql");
        private readonly Database database;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetPaymentMethodsByStore(
            Database database,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.database = database;
            this.limit = limit;
            this.skip = skip;
        }

        public async Task<Try<Paged<Try<(PaymentMethod PaymentMethod, uint Count)>>>> GetResult(string email, string store, Filter filter)
        {
            var parameters = new
            {
                email,
                store,
                skip = this.skip.Apply(filter),
                limit = this.limit.Apply(filter),
            };

            var data = await this.database.Execute(MapFrom, Statement.GetOrElse(Empty), parameters).ConfigureAwait(false);

            return data.Match<Try<Paged<Try<(PaymentMethod, uint)>>>>(
                _ => _,
                items => new Paged<Try<(PaymentMethod, uint)>>(items.ToList(), parameters.skip, parameters.limit));
        }
    }
}
