namespace Hawk.WebApi.Features.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.PaymentMethod;
    using Hawk.Domain.PaymentMethod.Commands;
    using Hawk.Domain.PaymentMethod.Queries;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.PaymentMethod.Queries.GetPaymentMethodsByPayeeParam;
    using static Hawk.Domain.Shared.Commands.DeleteParam<string>;
    using static Hawk.Domain.Shared.Commands.UpsertParam<string, Hawk.Domain.PaymentMethod.PaymentMethod>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.PaymentMethod.PaymentMethodModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class PaymentMethodsController : BaseController
    {
        private readonly IGetPaymentMethods getPaymentMethods;
        private readonly IGetPaymentMethodsByPayee getPaymentMethodsByPayee;
        private readonly IGetPaymentMethodByName getPaymentMethodByName;
        private readonly IUpsertPaymentMethod upsertPaymentMethod;
        private readonly IDeletePaymentMethod deletePaymentMethod;
        private readonly IGetPayeeByName getPayeeByName;
        private readonly CreatePaymentMethodModelValidator validator;

        public PaymentMethodsController(
            IGetPaymentMethods getPaymentMethods,
            IGetPaymentMethodsByPayee getPaymentMethodsByPayee,
            IGetPaymentMethodByName getPaymentMethodByName,
            IUpsertPaymentMethod upsertPaymentMethod,
            IDeletePaymentMethod deletePaymentMethod,
            IGetPayeeByName getPayeeByName)
        {
            this.getPaymentMethods = getPaymentMethods;
            this.getPaymentMethodsByPayee = getPaymentMethodsByPayee;
            this.getPaymentMethodByName = getPaymentMethodByName;
            this.upsertPaymentMethod = upsertPaymentMethod;
            this.deletePaymentMethod = deletePaymentMethod;
            this.getPayeeByName = getPayeeByName;
            this.validator = new CreatePaymentMethodModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("payment-methods")]
        [ProducesResponseType(typeof(Try<Page<Try<PaymentMethodModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var entities = await this.getPaymentMethods.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<PaymentMethodModel>>>,
                page => this.Ok(page.ToPage(NewPaymentMethodModel)));
        }

        /// <summary>
        /// Get by payee.
        /// </summary>
        /// <param name="payee"></param>
        /// <returns></returns>
        [HttpGet("payees/{payee}/payment-methods")]
        [ProducesResponseType(typeof(Try<Page<Try<PaymentMethodModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentMethodsByPayee(string payee)
        {
            var payeeEntity = await this.getPayeeByName.GetResult(NewGetByIdParam(this.GetUser(), payee));
            if (!payeeEntity)
            {
                return this.Error<PaymentMethodModel>(new NotFoundException("Payee not found."));
            }

            var entities = await this.getPaymentMethodsByPayee.GetResult(NewGetPaymentMethodsByPayeeParam(this.GetUser(), payeeEntity, this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<PaymentMethodModel>>>,
                page => this.Ok(page.ToPage(NewPaymentMethodModel)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("payment-methods/{name}")]
        [ProducesResponseType(typeof(Try<PaymentMethodModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPaymentMethodByName([FromRoute] string name)
        {
            var entity = await this.getPaymentMethodByName.GetResult(NewGetByIdParam(this.GetUser(), name));

            return entity.Match(
                this.Error<PaymentMethodModel>,
                paymentMethod => this.Ok(Success(NewPaymentMethodModel(paymentMethod))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("payment-methods")]
        [ProducesResponseType(typeof(Try<PaymentMethodModel>), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<PaymentMethodModel>(new InvalidObjectException("Invalid payment method.", validated));
            }

            if (await this.getPaymentMethodByName.GetResult(NewGetByIdParam(this.GetUser(), request.Name)))
            {
                return this.Error<PaymentMethodModel>(new AlreadyExistsException("Payment method already exists."));
            }

            Option<PaymentMethod> entity = request;
            var @try = await this.upsertPaymentMethod.Execute(NewUpsertParam(this.GetUser(), entity));

            return @try.Match(
                this.Error<PaymentMethodModel>,
                _ => this.Created(entity.Get().Id, Success(NewPaymentMethodModel(entity.Get()))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("payment-methods/{name}")]
        [ProducesResponseType(typeof(Try<PaymentMethodModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePaymentMethod(
            [FromRoute] string name,
            [FromBody] CreatePaymentMethodModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<PaymentMethodModel>(new InvalidObjectException("Invalid payment method.", validated));
            }

            var entity = await this.getPaymentMethodByName.GetResult(NewGetByIdParam(this.GetUser(), name));

            Option<PaymentMethod> newEntity = request;
            var @try = await this.upsertPaymentMethod.Execute(NewUpsertParam(this.GetUser(), name, newEntity));

            return @try.Match(
                this.Error<PaymentMethodModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewPaymentMethodModel(newEntity.Get()))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("payment-methods/{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePaymentMethod([FromRoute] string name)
        {
            var deleted = await this.deletePaymentMethod.Execute(NewDeleteParam(this.GetUser(), name));

            return deleted.Match(
                this.Error<PaymentMethodModel>,
                _ => this.NoContent());
        }
    }
}
