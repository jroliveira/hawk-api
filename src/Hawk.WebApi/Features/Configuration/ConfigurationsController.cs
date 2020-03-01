namespace Hawk.WebApi.Features.Configuration
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Configuration;
    using Hawk.Domain.Configuration.Commands;
    using Hawk.Domain.Configuration.Queries;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Shared.Commands.UpsertParam<string, Hawk.Domain.Configuration.Configuration>;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Configuration.ConfigurationModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("configurations")]
    public class ConfigurationsController : BaseController
    {
        private readonly IGetConfigurationByDescription getConfigurationByDescription;
        private readonly IUpsertConfiguration upsertConfiguration;
        private readonly Func<Try<Email>, CreateConfigurationModel, Task<ValidationResult>> validate;

        public ConfigurationsController(
            IGetConfigurationByDescription getConfigurationByDescription,
            IUpsertConfiguration upsertConfiguration,
            IGetCategoryByName getCategoryByName,
            IGetCurrencyByName getCurrencyByName,
            IGetPayeeByName getPayeeByName,
            IGetPaymentMethodByName getPaymentMethodByName)
        {
            this.getConfigurationByDescription = getConfigurationByDescription;
            this.upsertConfiguration = upsertConfiguration;

            this.validate = (email, request) =>
            {
                var validator = new CreateConfigurationModelValidator(
                    email.Get(),
                    getCategoryByName,
                    getCurrencyByName,
                    getPayeeByName,
                    getPaymentMethodByName);

                return validator.ValidateAsync(request);
            };
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
            var entity = await this.getConfigurationByDescription.GetResult(NewGetByIdParam(this.GetUser(), description));

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
            var validated = await this.validate(this.GetUser(), request);
            if (!validated.IsValid)
            {
                return this.Error<ConfigurationModel>(new InvalidObjectException("Invalid configuration.", validated));
            }

            var entity = await this.getConfigurationByDescription.GetResult(NewGetByIdParam(this.GetUser(), description));

            Option<Configuration> newEntity = request;
            var @try = await this.upsertConfiguration.Execute(NewUpsertParam(this.GetUser(), description, newEntity));

            return @try.Match(
                this.Error<ConfigurationModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewConfigurationModel(newEntity.Get()))));
        }
    }
}
