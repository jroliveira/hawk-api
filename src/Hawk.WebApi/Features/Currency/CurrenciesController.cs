namespace Hawk.WebApi.Features.Currency
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Currency.Commands;
    using Hawk.Domain.Currency.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Shared.Commands.DeleteParam<string>;
    using static Hawk.Domain.Shared.Commands.UpsertParam<string, Hawk.Domain.Currency.Currency>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Currency.CurrencyModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("currencies")]
    public class CurrenciesController : BaseController
    {
        private readonly IGetCurrencies getCurrencies;
        private readonly IGetCurrencyByCode getCurrencyByCode;
        private readonly IUpsertCurrency upsertCurrency;
        private readonly IDeleteCurrency deleteCurrency;
        private readonly Func<Try<Email>, string, CreateCurrencyModel, Task<ValidationResult>> validate;

        public CurrenciesController(
            IGetCurrencies getCurrencies,
            IGetCurrencyByCode getCurrencyByCode,
            IUpsertCurrency upsertCurrency,
            IDeleteCurrency deleteCurrency,
            Func<Try<Email>, string, CreateCurrencyModel, Task<ValidationResult>> validate)
        {
            this.getCurrencies = getCurrencies;
            this.getCurrencyByCode = getCurrencyByCode;
            this.upsertCurrency = upsertCurrency;
            this.deleteCurrency = deleteCurrency;
            this.validate = validate;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Try<Page<Try<CurrencyModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCurrencies()
        {
            var entities = await this.getCurrencies.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<CurrencyModel>>>,
                page => this.Ok(page.ToPage(NewCurrencyModel)));
        }

        /// <summary>
        /// Get by code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(typeof(Try<CurrencyModel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCurrencyByCode([FromRoute] string code)
        {
            var entity = await this.getCurrencyByCode.GetResult(NewGetByIdParam(this.GetUser(), code));

            return entity.Match(
                this.Error<CurrencyModel>,
                currency => this.Ok(Success(NewCurrencyModel(currency))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Try<CurrencyModel>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyModel request)
        {
            var validated = await this.validate(this.GetUser(), request.Code, request);
            if (!validated.IsValid)
            {
                return this.Error<CurrencyModel>(new InvalidObjectException("Invalid currency.", validated));
            }

            if (await this.getCurrencyByCode.GetResult(NewGetByIdParam(this.GetUser(), request.Code)))
            {
                return this.Error<CurrencyModel>(new AlreadyExistsException("Currency already exists."));
            }

            Option<Currency> entity = request;
            var @try = await this.upsertCurrency.Execute(NewUpsertParam(this.GetUser(), entity));

            return @try.Match(
                this.Error<CurrencyModel>,
                _ => this.Created(entity.Get().Id, Success(NewCurrencyModel(entity.Get()))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(typeof(Try<CurrencyModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCurrency(
            [FromRoute] string code,
            [FromBody] CreateCurrencyModel request)
        {
            var validated = await this.validate(this.GetUser(), code, request);
            if (!validated.IsValid)
            {
                return this.Error<CurrencyModel>(new InvalidObjectException("Invalid currency.", validated));
            }

            if (await this.getCurrencyByCode.GetResult(NewGetByIdParam(this.GetUser(), request.Code)))
            {
                return this.Error<CurrencyModel>(new AlreadyExistsException("Currency already exists."));
            }

            var entity = await this.getCurrencyByCode.GetResult(NewGetByIdParam(this.GetUser(), code));
            Option<Currency> newEntity = request;
            var @try = await this.upsertCurrency.Execute(NewUpsertParam(this.GetUser(), code, newEntity));

            return @try.Match(
                this.Error<CurrencyModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewCurrencyModel(newEntity.Get()))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete("{code}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCurrency([FromRoute] string code)
        {
            var deleted = await this.deleteCurrency.Execute(NewDeleteParam(this.GetUser(), code));

            return deleted.Match(
                this.Error<CurrencyModel>,
                _ => this.NoContent());
        }
    }
}
