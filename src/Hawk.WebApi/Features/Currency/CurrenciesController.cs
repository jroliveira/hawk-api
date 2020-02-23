namespace Hawk.WebApi.Features.Currency
{
    using System.Threading.Tasks;

    using Hawk.Domain.Currency;
    using Hawk.Domain.Currency.Commands;
    using Hawk.Domain.Currency.Queries;
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
        private readonly IGetCurrencyByName getCurrencyByName;
        private readonly IUpsertCurrency upsertCurrency;
        private readonly IDeleteCurrency deleteCurrency;
        private readonly CreateCurrencyModelValidator validator;

        public CurrenciesController(
            IGetCurrencies getCurrencies,
            IGetCurrencyByName getCurrencyByName,
            IUpsertCurrency upsertCurrency,
            IDeleteCurrency deleteCurrency)
        {
            this.getCurrencies = getCurrencies;
            this.getCurrencyByName = getCurrencyByName;
            this.upsertCurrency = upsertCurrency;
            this.deleteCurrency = deleteCurrency;
            this.validator = new CreateCurrencyModelValidator();
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
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Try<CurrencyModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCurrencyByName([FromRoute] string name)
        {
            var entity = await this.getCurrencyByName.GetResult(NewGetByIdParam(this.GetUser(), name));

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
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCurrency([FromBody] CreateCurrencyModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<CurrencyModel>(new InvalidObjectException("Invalid currency.", validated));
            }

            if (await this.getCurrencyByName.GetResult(NewGetByIdParam(this.GetUser(), request.Name)))
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
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{name}")]
        [ProducesResponseType(typeof(Try<CurrencyModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCurrency(
            [FromRoute] string name,
            [FromBody] CreateCurrencyModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<CurrencyModel>(new InvalidObjectException("Invalid currency.", validated));
            }

            var entity = await this.getCurrencyByName.GetResult(NewGetByIdParam(this.GetUser(), name));

            Option<Currency> newEntity = request;
            var @try = await this.upsertCurrency.Execute(NewUpsertParam(this.GetUser(), name, newEntity));

            return @try.Match(
                this.Error<CurrencyModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewCurrencyModel(newEntity.Get()))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCurrency([FromRoute] string name)
        {
            var deleted = await this.deleteCurrency.Execute(NewDeleteParam(this.GetUser(), name));

            return deleted.Match(
                this.Error<CurrencyModel>,
                _ => this.NoContent());
        }
    }
}
