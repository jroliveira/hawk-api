namespace Hawk.Infrastructure.Data.Neo4J
{
    using System.Linq;
    using System.Threading.Tasks;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class CheckNeo4J : ICheckNeo4J
    {
        private readonly Neo4JConnection connection;

        public CheckNeo4J(Neo4JConnection connection) => this.connection = connection;

        public async Task<bool> Execute()
        {
            var @checked = await this.connection.ExecuteCypherScalar(
                record => Success(true),
                "MATCH (n) RETURN n LIMIT 1",
                default);

            return @checked.Fold(false)(_ => _);
        }
    }
}
