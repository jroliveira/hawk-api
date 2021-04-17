namespace Hawk.Domain.Budget.Data.Neo4J
{
    using Hawk.Domain.Budget.Commands;
    using Hawk.Domain.Budget.Data.Neo4J.Commands;
    using Hawk.Domain.Budget.Data.Neo4J.Queries;
    using Hawk.Domain.Budget.Queries;
    using Hawk.Infrastructure.Filter;

    using Http.Query.Filter;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureBudgetWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertBudget, UpsertBudget>()
            .AddScoped<IDeleteBudget, DeleteBudget>()
            .AddScoped<IGetBudgets, GetBudgets>()
            .AddScoped<IGetBudgetById, GetBudgetById>()
            .AddSingleton<IWhere<string, Filter>, Where>()
            .AddSingleton<IWhere<string, Filter>, WhereTransaction>();
    }
}
