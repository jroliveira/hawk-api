namespace Hawk.Domain.Budget
{
    using Hawk.Domain.Budget.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureBudget(this IServiceCollection @this) => @this
            .ConfigureBudgetWithNeo4J();
    }
}
