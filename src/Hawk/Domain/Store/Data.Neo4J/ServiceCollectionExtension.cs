namespace Hawk.Domain.Store.Data.Neo4J
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
