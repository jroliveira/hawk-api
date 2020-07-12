namespace Hawk.WebApi.Features.Payee
{
    using System;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Category.Queries;
    using Hawk.Domain.Payee;
    using Hawk.Domain.Payee.Commands;
    using Hawk.Domain.Payee.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Payee.Queries.GetPayeesByCategoryParam;
    using static Hawk.Domain.Shared.Commands.DeleteParam<string>;
    using static Hawk.Domain.Shared.Commands.UpsertParam<string, Hawk.Domain.Payee.Payee>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<string>;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Payee.PayeeModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class PayeesController : BaseController
    {
        private readonly IGetPayees getPayees;
        private readonly IGetPayeesByCategory getPayeesByCategory;
        private readonly IGetPayeeByName getPayeeByName;
        private readonly IUpsertPayee upsertPayee;
        private readonly IDeletePayee deletePayee;
        private readonly IGetCategoryByName getCategoryByName;
        private readonly Func<Try<Email>, string, CreatePayeeModel, Task<ValidationResult>> validate;

        public PayeesController(
            IGetPayees getPayees,
            IGetPayeesByCategory getPayeesByCategory,
            IGetPayeeByName getPayeeByName,
            IUpsertPayee upsertPayee,
            IDeletePayee deletePayee,
            IGetCategoryByName getCategoryByName,
            Func<Try<Email>, string, CreatePayeeModel, Task<ValidationResult>> validate)
        {
            this.getPayees = getPayees;
            this.getPayeesByCategory = getPayeesByCategory;
            this.getPayeeByName = getPayeeByName;
            this.upsertPayee = upsertPayee;
            this.deletePayee = deletePayee;
            this.getCategoryByName = getCategoryByName;
            this.validate = validate;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("payees")]
        [ProducesResponseType(typeof(Try<Page<Try<PayeeModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPayees()
        {
            var entities = await this.getPayees.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<PayeeModel>>>,
                page => this.Ok(page.ToPage(entity => NewPayeeModel(entity))));
        }

        /// <summary>
        /// Get by category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet("categories/{category}/payees")]
        [ProducesResponseType(typeof(Try<Page<Try<PayeeModel>>>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPayeesByCategory(string category)
        {
            var categoryEntity = await this.getCategoryByName.GetResult(NewGetByIdParam(this.GetUser(), category));
            if (!categoryEntity)
            {
                return this.Error<PayeeModel>(new NotFoundException("Category not found."));
            }

            var entities = await this.getPayeesByCategory.GetResult(NewGetPayeesByCategoryParam(this.GetUser(), categoryEntity, this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<PayeeModel>>>,
                page => this.Ok(page.ToPage(entity => NewPayeeModel(entity))));
        }

        /// <summary>
        /// Get by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("payees/{name}")]
        [ProducesResponseType(typeof(Try<PayeeModel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetPayeeByName([FromRoute] string name)
        {
            var entity = await this.getPayeeByName.GetResult(NewGetByIdParam(this.GetUser(), name));

            return entity.Match(
                this.Error<PayeeModel>,
                payee => this.Ok(Success(NewPayeeModel(payee))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("payees")]
        [ProducesResponseType(typeof(Try<PayeeModel>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreatePayee([FromBody] CreatePayeeModel request)
        {
            var validated = await this.validate(this.GetUser(), request.Name, request);
            if (!validated.IsValid)
            {
                return this.Error<PayeeModel>(new InvalidObjectException("Invalid payee.", validated));
            }

            if (await this.getPayeeByName.GetResult(NewGetByIdParam(this.GetUser(), request.Name)))
            {
                return this.Error<PayeeModel>(new AlreadyExistsException("Payee already exists."));
            }

            Option<Payee> entity = request;
            var @try = await this.upsertPayee.Execute(NewUpsertParam(this.GetUser(), entity));

            return @try.Match(
                this.Error<PayeeModel>,
                _ => this.Created(entity.Get().Id, Success(NewPayeeModel(entity.Get()))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("payees/{name}")]
        [ProducesResponseType(typeof(Try<PayeeModel>), 201)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdatePayee(
            [FromRoute] string name,
            [FromBody] CreatePayeeModel request)
        {
            var validated = await this.validate(this.GetUser(), name, request);
            if (!validated.IsValid)
            {
                return this.Error<PayeeModel>(new InvalidObjectException("Invalid payee.", validated));
            }

            if (await this.getPayeeByName.GetResult(NewGetByIdParam(this.GetUser(), request.Name)))
            {
                return this.Error<PayeeModel>(new AlreadyExistsException("Payee already exists."));
            }

            var entity = await this.getPayeeByName.GetResult(NewGetByIdParam(this.GetUser(), name));
            Option<Payee> newEntity = request;
            var @try = await this.upsertPayee.Execute(NewUpsertParam(this.GetUser(), name, newEntity));

            return @try.Match(
                this.Error<PayeeModel>,
                _ => entity
                    ? this.NoContent()
                    : this.Created(newEntity.Get().Id, Success(NewPayeeModel(newEntity.Get()))));
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpDelete("payees/{name}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeletePayee([FromRoute] string name)
        {
            var deleted = await this.deletePayee.Execute(NewDeleteParam(this.GetUser(), name));

            return deleted.Match(
                this.Error<PayeeModel>,
                _ => this.NoContent());
        }
    }
}
