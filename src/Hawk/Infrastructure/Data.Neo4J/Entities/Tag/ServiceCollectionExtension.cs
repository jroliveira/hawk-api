namespace Hawk.Infrastructure.Data.Neo4J.Entities.Tag
{
    using Hawk.Domain.Tag;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureTagWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertTag, UpsertTag>()
            .AddScoped<IDeleteTag, DeleteTag>()
            .AddScoped<IGetTags, GetTags>()
            .AddScoped<IGetTagsByStore, GetTagsByStore>()
            .AddScoped<IGetTagByName, GetTagByName>();
    }
}
