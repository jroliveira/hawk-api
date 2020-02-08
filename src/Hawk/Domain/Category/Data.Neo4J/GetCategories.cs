namespace Hawk.Domain.Category.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.Category;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Filter;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;

    using Http.Query.Filter;

    using static System.IO.Path;

    using static Hawk.Domain.Category.Data.Neo4J.CategoryMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class GetCategories : IGetCategories
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Category", "Data.Neo4J", "GetCategories.cql"));
        private readonly Neo4JConnection connection;
        private readonly ILimit<int, Filter> limit;
        private readonly ISkip<int, Filter> skip;

        public GetCategories(
            Neo4JConnection connection,
            ILimit<int, Filter> limit,
            ISkip<int, Filter> skip)
        {
            this.connection = connection;
            this.limit = limit;
            this.skip = skip;
        }

        public Task<Try<Page<Try<Category>>>> GetResult(Option<Email> email, Filter filter) => email.Match(
            async some =>
            {
                var parameters = new
                {
                    email = some.Value,
                    skip = this.skip.Apply(filter),
                    limit = this.limit.Apply(filter),
                };

                var data = await this.connection.ExecuteCypher(MapCategory, Statement, parameters);

                return data.Match<Try<Page<Try<Category>>>>(
                    _ => _,
                    items => new Page<Try<Category>>(items, parameters.skip, parameters.limit));
            },
            () => Task(Failure<Page<Try<Category>>>(new NullObjectException("Email is required."))));
    }
}
