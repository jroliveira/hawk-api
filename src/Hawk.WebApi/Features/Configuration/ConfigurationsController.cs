namespace Hawk.WebApi.Features.Configuration
{
    using System.Threading.Tasks;

    using Hawk.Domain.Configuration;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Configuration.ConfigurationModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("configurations")]
    public class ConfigurationsController : BaseController
    {
        private readonly IGetConfigurationByDescription getConfigurationByDescription;
        private readonly IUpsertConfiguration upsertConfiguration;
        private readonly CreateConfigurationModelValidator validator;

        public ConfigurationsController(
            IGetConfigurationByDescription getConfigurationByDescription,
            IUpsertConfiguration upsertConfiguration)
        {
            this.getConfigurationByDescription = getConfigurationByDescription;
            this.upsertConfiguration = upsertConfiguration;
            this.validator = new CreateConfigurationModelValidator();
        }

        /// <summary>
        /// Get by description.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{description}")]
        [ProducesResponseType(typeof(Try<ConfigurationModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetConfigurationByDescription([FromRoute] string description)
        {
            var entity = await this.getConfigurationByDescription.GetResult(this.GetUser(), description);

            return entity.Match(
                this.Error<ConfigurationModel>,
                configuration => this.Ok(Success(NewConfigurationModel(configuration))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="description"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{description}")]
        [ProducesResponseType(typeof(Try<ConfigurationModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateConfiguration(
            [FromRoute] string description,
            [FromBody] CreateConfigurationModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<ConfigurationModel>(new InvalidObjectException("Invalid configuration.", validated));
            }

            var entity = await this.getConfigurationByDescription.GetResult(this.GetUser(), description);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertConfiguration.Execute(this.GetUser(), description, request);

                    return inserted.Match(
                        this.Error<ConfigurationModel>,
                        configuration => this.Created(Success(NewConfigurationModel(configuration))));
                },
                async _ =>
                {
                    var updated = await this.upsertConfiguration.Execute(this.GetUser(), description, request);

                    return updated.Match(
                        this.Error<ConfigurationModel>,
                        configuration => this.NoContent());
                });
        }
    }
}
