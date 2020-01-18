namespace Hawk.Domain.PaymentMethod.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetPaymentMethodByName : IGetPaymentMethodByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("PaymentMethod", "Data.Neo4J", "GetPaymentMethodByName.cql"));
        private readonly Neo4JConnection connection;

        public GetPaymentMethodByName(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<PaymentMethod>> GetResult(Email email, string name) => this.connection.ExecuteCypherScalar(
            MapPaymentMethod,
            Statement,
            new
            {
                email = email.Value,
                name,
            });
    }
}
