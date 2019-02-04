﻿namespace Hawk.WebApi.Features.Configuration
{
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Domain.Shared.Exceptions;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using static NewConfigurationModel;

    [Authorize]
    [ApiVersion("1")]
    [Route("configurations")]
    public class ConfigurationsController : BaseController
    {
        private readonly IGetConfigurationByDescription getConfigurationByDescription;
        private readonly IUpsertConfiguration upsertConfiguration;
        private readonly NewConfigurationModelValidator validator;

        public ConfigurationsController(
            IGetConfigurationByDescription getConfigurationByDescription,
            IUpsertConfiguration upsertConfiguration,
            IHostingEnvironment environment)
            : this(getConfigurationByDescription, upsertConfiguration, new NewConfigurationModelValidator(), environment)
        {
            this.getConfigurationByDescription = getConfigurationByDescription;
            this.upsertConfiguration = upsertConfiguration;
        }

        internal ConfigurationsController(
            IGetConfigurationByDescription getConfigurationByDescription,
            IUpsertConfiguration upsertConfiguration,
            NewConfigurationModelValidator validator,
            IHostingEnvironment environment)
            : base(environment)
        {
            this.getConfigurationByDescription = getConfigurationByDescription;
            this.upsertConfiguration = upsertConfiguration;
            this.validator = validator;
        }

        /// <summary>
        /// Get by description.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{description}")]
        [ProducesResponseType(typeof(ConfigurationModel), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByDescription(
            [FromRoute] string description)
        {
            var entity = await this.getConfigurationByDescription.GetResult(this.GetUser(), description);

            return entity.Match(
                this.HandleError,
                configuration => this.Ok(new ConfigurationModel(configuration)));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{description}")]
        [ValidateSchema(typeof(NewConfigurationModel))]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(
            [FromRoute] string description,
            [FromBody] NewConfigurationModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.HandleError(new InvalidObjectException("Invalid configuration.", validated));
            }

            var entity = await this.getConfigurationByDescription.GetResult(this.GetUser(), description);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertConfiguration.Execute(this.GetUser(), MapFrom(description, request));

                    return inserted.Match(
                        this.HandleError,
                        configuration => this.Created(default, new ConfigurationModel(configuration)));
                },
                async _ =>
                {
                    var updated = await this.upsertConfiguration.Execute(this.GetUser(), MapFrom(description, request));

                    return updated.Match(
                        this.HandleError,
                        configuration => this.NoContent());
                });
        }
    }
}