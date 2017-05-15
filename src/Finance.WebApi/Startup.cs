namespace Finance.WebApi
{
    using System;
    using System.Text;

    using AutoMapper;

    using Finance.Infrastructure;
    using Finance.Infrastructure.Filter;
    using Finance.WebApi.Lib.Middlewares;
    using Finance.WebApi.Lib.Validators;

    using Http.Query.Filter;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Cors.Internal;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    using Newtonsoft.Json;

    using Neo4j = Finance.Infrastructure.Data.Neo4j;

    public class Startup
    {
        private const string SecretKey = "needtogetthisfromenvironment";
        private readonly SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.Configure<Neo4j.Config>(this.Configuration.GetSection("Neo4j"));
            services.AddSingleton<Neo4j.Database>();
            services.AddSingleton<File, Neo4j.GetScript>();
            services.AddSingleton<PartialUpdater>();

            // Filters
            services.AddSingleton<IWhere<string, Filter>, Neo4j.Filter.Where>();
            services.AddSingleton<ISkip<int, Filter>, Neo4j.Filter.Skip>();
            services.AddSingleton<ILimit<int, Filter>, Neo4j.Filter.Limit>();

            // Commands
            services.AddSingleton<Neo4j.Commands.Account.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Transaction.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Transaction.ExcludeCommand>();
            services.AddSingleton<Neo4j.Commands.Currency.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.PaymentMethod.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Store.CreateCommand>();
            services.AddSingleton<Neo4j.Commands.Tag.CreateCommand>();

            // Queries
            services.AddSingleton<Neo4j.Queries.Account.GetByEmailQuery>();
            services.AddSingleton<Neo4j.Queries.Transaction.GetAllQuery>();
            services.AddSingleton<Neo4j.Queries.Transaction.GetByIdQuery>();
            services.AddSingleton<Neo4j.Queries.Tag.GetAllQuery>();

            // Mappings
            services.AddSingleton<Neo4j.Mappings.AccountMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.TransactionMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.ParcelMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.StoreMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.TransactionMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.TagMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.Payment.CurrencyMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.Payment.MethodMapping>();
            services.AddSingleton<Neo4j.Mappings.Transaction.Payment.PaymentMapping>();

            // Validators
            services.AddSingleton<AccountValidator>();
            services.AddSingleton<TransactionValidator>();

            services.AddAutoMapper();
            services
                .AddMvc(config =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    //config.Filters.Add(new AuthorizeFilter(policy));
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.Formatting = Formatting.Indented;
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
            });

            // Get options from app settings
            var jwtAppSettingOptions = this.Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(this.signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware<HandlerErrorMiddleware>();

            var jwtAppSettingOptions = this.Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = this.signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseCors("CorsPolicy");

            app.UseMvc();
        }
    }
}
