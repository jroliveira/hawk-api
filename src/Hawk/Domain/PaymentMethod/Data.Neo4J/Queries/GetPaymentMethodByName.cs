namespace Hawk.Domain.PaymentMethod.Data.Neo4J.Queries
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetPaymentMethodByName : Query<GetByIdParam<string>, PaymentMethod>, IGetPaymentMethodByName
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("PaymentMethod", "Data.Neo4J", "Queries", "GetPaymentMethodByName.cql"));
        private readonly Neo4JConnection connection;

        public GetPaymentMethodByName(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<PaymentMethod>> GetResult(GetByIdParam<string> param) => this.connection.ExecuteCypherScalar(
            record => MapPaymentMethod(record),
            StatementOption,
            new
            {
                email = param.Email.Value,
                name = param.Id,
            });
    }
}
