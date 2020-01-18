namespace Hawk.WebApi.Features.PaymentMethod
{
    using System.Threading.Tasks;

    using Hawk.Domain.PaymentMethod;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

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
        private readonly CreatePaymentMethodModelValidator validator;

        public PaymentMethodsController(
            IGetPaymentMethods getPaymentMethods,
            IGetPaymentMethodsByPayee getPaymentMethodsByPayee,
            IGetPaymentMethodByName getPaymentMethodByName,
            IUpsertPaymentMethod upsertPaymentMethod,
            IDeletePaymentMethod deletePaymentMethod)
        {
            this.getPaymentMethods = getPaymentMethods;
            this.getPaymentMethodsByPayee = getPaymentMethodsByPayee;
            this.getPaymentMethodByName = getPaymentMethodByName;
            this.upsertPaymentMethod = upsertPaymentMethod;
            this.deletePaymentMethod = deletePaymentMethod;
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
            var entities = await this.getPaymentMethods.GetResult(this.GetUser(), this.Request.QueryString.Value);

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
            var entities = await this.getPaymentMethodsByPayee.GetResult(this.GetUser(), payee, this.Request.QueryString.Value);

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
            var entity = await this.getPaymentMethodByName.GetResult(this.GetUser(), name);

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

            var entity = await this.getPaymentMethodByName.GetResult(this.GetUser(), request.Name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertPaymentMethod.Execute(this.GetUser(), request.Name, request);

                    return inserted.Match(
                        this.Error<PaymentMethodModel>,
                        paymentMethod => this.Created(paymentMethod.Value, Success(NewPaymentMethodModel(paymentMethod))));
                },
                _ => Task(this.Error<PaymentMethodModel>(new AlreadyExistsException("Payment method already exists."))));
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

            var entity = await this.getPaymentMethodByName.GetResult(this.GetUser(), name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertPaymentMethod.Execute(this.GetUser(), name, request);

                    return inserted.Match(
                        this.Error<PaymentMethodModel>,
                        paymentMethod => this.Created(Success(NewPaymentMethodModel(paymentMethod))));
                },
                async _ =>
                {
                    var updated = await this.upsertPaymentMethod.Execute(this.GetUser(), name, request);

                    return updated.Match(
                        this.Error<PaymentMethodModel>,
                        paymentMethod => this.NoContent());
                });
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
            var deleted = await this.deletePaymentMethod.Execute(this.GetUser(), name);

            return deleted.Match(
                this.Error<PaymentMethodModel>,
                _ => this.NoContent());
        }
    }
}
