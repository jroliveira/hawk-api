﻿namespace Finance.WebApi.Configuration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Neo4j = Finance.Infrastructure.Data.Neo4j;

    internal static class Database
    {
        internal static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<Neo4j.Config>(configuration.GetSection("Neo4j"));
            services.AddSingleton<Neo4j.Database>();
            services.AddSingleton<Neo4j.GetScript>();
            services.AddSingleton<Neo4j.Reports.GetScript>();

            return services;
        }
    }
}