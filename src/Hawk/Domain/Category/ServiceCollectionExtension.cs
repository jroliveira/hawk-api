namespace Hawk.Domain.Category
{
    using Hawk.Domain.Category.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCategory(this IServiceCollection @this) => @this
            .ConfigureCategoryWithNeo4J();
    }
}
