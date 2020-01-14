namespace Hawk.Infrastructure.Data.Neo4J.Entities.Store
{
    using Hawk.Domain.Store;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureStoreWithNeo4J(this IServiceCollection @this) => @this
            .AddScoped<IUpsertStore, UpsertStore>()
            .AddScoped<IDeleteStore, DeleteStore>()
            .AddScoped<IGetStores, GetStores>()
            .AddScoped<IGetStoreByName, GetStoreByName>();
    }
}
