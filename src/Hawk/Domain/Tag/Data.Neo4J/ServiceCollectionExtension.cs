namespace Hawk.Domain.Tag.Data.Neo4J
{
    using Hawk.Domain.Tag;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureTagWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertTag, UpsertTag>()
            .AddScoped<IDeleteTag, DeleteTag>()
            .AddScoped<IGetTags, GetTags>()
            .AddScoped<IGetTagsByPayee, GetTagsByPayee>()
            .AddScoped<IGetTagByName, GetTagByName>();
    }
}
