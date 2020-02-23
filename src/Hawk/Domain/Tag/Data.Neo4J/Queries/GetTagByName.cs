namespace Hawk.Domain.Tag.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared.Queries;
    using Hawk.Domain.Tag;
    using Hawk.Domain.Tag.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Tag.Data.Neo4J.TagMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetTagByName : Query<GetByIdParam<string>, Tag>, IGetTagByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("Tag", "Data.Neo4J", "Queries", "GetTagByName.cql"));
        private readonly Neo4JConnection connection;

        public GetTagByName(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Tag>> GetResult(GetByIdParam<string> param) => this.connection.ExecuteCypherScalar(
            MapTag,
            Statement,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
