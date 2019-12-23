namespace Hawk.WebApi.Infrastructure.Authentication
{
    using System.Collections.Generic;

    using Hawk.WebApi.Infrastructure.Authentication.Configurations;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using NSwag;
    using NSwag.Generation.AspNetCore;
    using NSwag.Generation.Processors.Security;

    using static System.Linq.Enumerable;

    using static Flurl.Url;

    using static NSwag.OpenApiOAuth2Flow;
    using static NSwag.OpenApiSecuritySchemeType;

    internal static class SwaggerDocumentExtension
    {
        internal static void AddSecurity(this AspNetCoreOpenApiDocumentGeneratorSettings @this, IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("authentication")
                .Get<AuthConfiguration>();

            if (!authConfig.IsEnabled())
            {
                return;
            }

            @this.AddSecurity("bearer", Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OAuth2,
                Description = "Severino",
                Flow = Password,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = Combine(authConfig.Authority?.Uri, "connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "hawk", "Admin" },
                            { "hawk.readonly", "Read-only" },
                        },
                    },
                },
            });

            @this.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
        }
    }
}
