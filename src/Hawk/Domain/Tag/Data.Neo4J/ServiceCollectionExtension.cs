namespace Hawk.Domain.Tag.Data.Neo4J
{
    using Hawk.Domain.Tag.Commands;
    using Hawk.Domain.Tag.Data.Neo4J.Commands;
    using Hawk.Domain.Tag.Data.Neo4J.Queries;
    using Hawk.Domain.Tag.Queries;

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
