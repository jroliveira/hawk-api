namespace Hawk.Domain.Tag
{
    using Hawk.Domain.Tag.Data.Neo4J;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureTag(this IServiceCollection @this) => @this
            .ConfigureTagWithNeo4J();
    }
}
