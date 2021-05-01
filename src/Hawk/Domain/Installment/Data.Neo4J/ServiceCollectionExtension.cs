namespace Hawk.Domain.Installment.Data.Neo4J
{
    using Hawk.Domain.Installment.Commands;
    using Hawk.Domain.Installment.Data.Neo4J.Commands;
    using Hawk.Domain.Installment.Data.Neo4J.Queries;
    using Hawk.Domain.Installment.Queries;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureInstallmentWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IDeleteInstallment, DeleteInstallment>()
            .AddScoped<IGetInstallments, GetInstallments>()
            .AddScoped<IGetInstallmentById, GetInstallmentById>();
    }
}
