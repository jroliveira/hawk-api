namespace Hawk.Domain.Category.Data.Neo4J
{
    using Hawk.Domain.Category.Commands;
    using Hawk.Domain.Category.Data.Neo4J.Commands;
    using Hawk.Domain.Category.Data.Neo4J.Queries;
    using Hawk.Domain.Category.Queries;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCategoryWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertCategory, UpsertCategory>()
            .AddScoped<IDeleteCategory, DeleteCategory>()
            .AddScoped<IGetCategories, GetCategories>()
            .AddScoped<IGetCategoriesByPayee, GetCategoriesByPayee>()
            .AddScoped<IGetCategoryByName, GetCategoryByName>();
    }
}
