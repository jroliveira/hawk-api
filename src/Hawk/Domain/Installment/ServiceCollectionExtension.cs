namespace Hawk.Domain.Installment
{
    using Hawk.Domain.Installment.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureInstallment(this IServiceCollection @this) => @this
            .ConfigureInstallmentWithNeo4J();
    }
}
