namespace Hawk.Domain.Installment.Data.Neo4J.Queries
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Installment;
    using Hawk.Domain.Installment.Queries;
    using Hawk.Domain.Shared.Queries;
    using Hawk.Infrastructure.Data.Neo4J;
    using Hawk.Infrastructure.Monad;

    using static System.IO.Path;

    using static Hawk.Domain.Installment.Data.Neo4J.InstallmentMapping;
    using static Hawk.Infrastructure.Data.Neo4J.CypherScript;

    internal sealed class GetInstallmentById : Query<GetByIdParam<Guid>, Installment>, IGetInstallmentById
    {
        private static readonly Option<string> StatementOption = ReadCypherScript(Combine("Installment", "Data.Neo4J", "Queries", "GetInstallmentById.cql"));
        private readonly Neo4JConnection connection;

        public GetInstallmentById(Neo4JConnection connection) => this.connection = connection;

        protected override Task<Try<Installment>> GetResult(GetByIdParam<Guid> param) => this.connection.ExecuteCypherScalar(
            record => MapInstallment(record),
            StatementOption,
            new
            {
                email = param.Email.Value,
                id = param.Id.ToString(),
            });
    }
}
