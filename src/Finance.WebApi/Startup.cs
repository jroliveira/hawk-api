namespace Finance.WebApi
{
    using System;
    using System.Text;

    using AutoMapper;

    using Finance.Entities.Transaction;
    using Finance.Infrastructure;
    using Finance.Infrastructure.Data.Filter;
    using Finance.Infrastructure.Data.Filter.Linq;
    using Finance.WebApi.Lib.Middlewares;
    using Finance.WebApi.Lib.Validators;

    using Http.Query.Filter;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    using Newtonsoft.Json;

    using Data = Finance.Infrastructure.Data;

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

            services.AddScoped<PartialUpdater>();

            // Fields
            services.AddScoped<ISkip<int, Filter>, Skip>();
            services.AddScoped<ILimit<int, Filter>, Limit>();
            services.AddScoped<IWhere<bool, Filter, Transaction>, Where>();

            // Commands
            services.AddScoped<Data.Commands.Account.CreateCommand>();
            services.AddScoped<Data.Commands.Account.UpdateCommand>();
            services.AddScoped<Data.Commands.Account.ExcludeCommand>();
            services.AddScoped<Data.Commands.Transaction.CreateCommand>();
            services.AddScoped<Data.Commands.Transaction.ExcludeCommand>();

            // Queries
            services.AddScoped<Data.Queries.Account.GetByEmailQuery>();
            services.AddScoped<Data.Queries.Transaction.GetAllQuery>();
            services.AddScoped<Data.Queries.Transaction.GetByIdQuery>();

            // Validators
            services.AddScoped<AccountValidator>();
            services.AddScoped<TransactionValidator>();

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

            app.UseMvc();
        }
    }
}
