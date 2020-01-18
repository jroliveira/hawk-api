namespace Hawk.WebApi.Features.Payee
{
    using System.Threading.Tasks;

    using Hawk.Domain.Payee;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Payee.PayeeModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("payees")]
    public class PayeesController : BaseController
    {
        private readonly IGetPayees getPayees;
        private readonly IGetPayeeByName getPayeeByName;
        private readonly IUpsertPayee upsertPayee;
        private readonly IDeletePayee deletePayee;
        private readonly CreatePayeeModelValidator validator;

        public PayeesController(
            IGetPayees getPayees,
            IGetPayeeByName getPayeeByName,
            IUpsertPayee upsertPayee,
            IDeletePayee deletePayee)
        {
            this.getPayees = getPayees;
            this.getPayeeByName = getPayeeByName;
            this.upsertPayee = upsertPayee;
            this.deletePayee = deletePayee;
            this.validator = new CreatePayeeModelValidator();
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Try<Page<Try<PayeeModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPayees()
        {
            var entities = await this.getPayees.GetResult(this.GetUser(), this.Request.QueryString.Value);

            return entities.Match(
                this.Error<Page<Try<PayeeModel>>>,
                page => this.Ok(page.ToPage(NewPayeeModel)));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(Try<PayeeModel>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPayeeByName([FromRoute] string name)
        {
            var entity = await this.getPayeeByName.GetResult(this.GetUser(), name);

            return entity.Match(
                this.Error<PayeeModel>,
                payee => this.Ok(Success(NewPayeeModel(payee))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Try<PayeeModel>), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePayee([FromBody] CreatePayeeModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<PayeeModel>(new InvalidObjectException("Invalid payee.", validated));
            }

            var entity = await this.getPayeeByName.GetResult(this.GetUser(), request.Name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertPayee.Execute(this.GetUser(), request.Name, request);

                    return inserted.Match(
                        this.Error<PayeeModel>,
                        payee => this.Created(payee.Value, Success(NewPayeeModel(payee))));
                },
                _ => Task(this.Error<PayeeModel>(new AlreadyExistsException("Payee already exists."))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{name}")]
        [ProducesResponseType(typeof(Try<PayeeModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePayee(
            [FromRoute] string name,
            [FromBody] CreatePayeeModel request)
        {
            var validated = await this.validator.ValidateAsync(request);
            if (!validated.IsValid)
            {
                return this.Error<PayeeModel>(new InvalidObjectException("Invalid payee.", validated));
            }

            var entity = await this.getPayeeByName.GetResult(this.GetUser(), name);

            return await entity.Match(
                async _ =>
                {
                    var inserted = await this.upsertPayee.Execute(this.GetUser(), name, request);

                    return inserted.Match(
                        this.Error<PayeeModel>,
                        payee => this.Created(Success(NewPayeeModel(payee))));
                },
                async _ =>
                {
                    var updated = await this.upsertPayee.Execute(this.GetUser(), name, request);

                    return updated.Match(
                        this.Error<PayeeModel>,
                        payee => this.NoContent());
                });
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
        public async Task<IActionResult> DeletePayee([FromRoute] string name)
        {
            var deleted = await this.deletePayee.Execute(this.GetUser(), name);

            return deleted.Match(
                this.Error<PayeeModel>,
                _ => this.NoContent());
        }
    }
}
