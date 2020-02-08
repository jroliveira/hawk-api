namespace Hawk.Domain.PaymentMethod.Data.Neo4J
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.PaymentMethod.Data.Neo4J.PaymentMethodMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    internal sealed class GetPaymentMethodByName : IGetPaymentMethodByName
    {
        private static readonly Option<string> Statement = ReadCypherScript(Combine("PaymentMethod", "Data.Neo4J", "GetPaymentMethodByName.cql"));
        private readonly Neo4JConnection connection;

        public GetPaymentMethodByName(Neo4JConnection connection) => this.connection = connection;

        public Task<Try<PaymentMethod>> GetResult(Option<Email> email, Option<string> name) =>
            email
            && name
                ? this.connection.ExecuteCypherScalar(
                    MapPaymentMethod,
                    Statement,
                    new
                    {
                        email = email.Get().Value,
                        name = name.Get(),
                    })
                : Task(Failure<PaymentMethod>(new NullObjectException("Parameters are required.")));
    }
}
