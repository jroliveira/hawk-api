namespace Hawk.WebApi.Features.Budget
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using FluentValidation.Results;

    using Hawk.Domain.Budget;
    using Hawk.Domain.Budget.Commands;
    using Hawk.Domain.Budget.Queries;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Budget.Queries.GetBudgetByIdParam;

    using static Hawk.Domain.Shared.Commands.DeleteParam<System.Guid>;
    using static Hawk.Domain.Shared.Commands.UpsertParam<System.Guid, Hawk.Domain.Budget.Budget>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Budget.BudgetModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("budgets")]
    public class BudgetsController : BaseController
    {
        private readonly IGetBudgets getBudgets;
        private readonly IGetBudgetById getBudgetById;
        private readonly IUpsertBudget upsertBudget;
        private readonly IDeleteBudget deleteBudget;
        private readonly Func<Try<Email>, string?, CreateBudgetModel, Task<ValidationResult>> validate;

        public BudgetsController(
            IGetBudgets getBudgets,
            IGetBudgetById getBudgetById,
            IUpsertBudget upsertBudget,
            IDeleteBudget deleteBudget,
            Func<Try<Email>, string?, CreateBudgetModel, Task<ValidationResult>> validate)
        {
            this.getBudgets = getBudgets;
            this.getBudgetById = getBudgetById;
            this.upsertBudget = upsertBudget;
            this.deleteBudget = deleteBudget;
            this.validate = validate;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Try<Page<Try<BudgetModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBudgets()
        {
            var entities = await this.getBudgets.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<BudgetModel>>>,
                page => this.Ok(page.ToPage(NewBudgetModel)));
        }

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Try<BudgetModel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetBudgetById([FromRoute] string id)
        {
            var entity = await this.getBudgetById.GetResult(NewGetBudgetByIdParam(this.GetUser(), new Guid(id), this.Request.QueryString.Value));

            return entity.Match(
                this.Error<BudgetModel>,
                budget => this.Ok(Success(NewBudgetModel(budget))));
        }

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Try<BudgetModel>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetModel request)
        {
            var validated = await this.validate(this.GetUser(), default, request);
            if (!validated.IsValid)
            {
                return this.Error<BudgetModel>(new InvalidObjectException("Invalid budget.", validated));
            }

            Option<Budget> entity = request;
            var @try = await this.upsertBudget.Execute(NewUpsertParam(this.GetUser(), entity));

            return @try.Match(
                this.Error<BudgetModel>,
                _ => this.Created(entity.Get().Id, Success(NewBudgetModel(entity.Get()))));
        }

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateBudget(
            [FromRoute] string id,
            [FromBody] CreateBudgetModel request)
        {
            var validated = await this.validate(this.GetUser(), id, request);
            if (!validated.IsValid)
            {
                return this.Error<BudgetModel>(new InvalidObjectException("Invalid budget.", validated));
            }

            var entity = await this.getBudgetById.GetResult(NewGetBudgetByIdParam(this.GetUser(), new Guid(id), this.Request.QueryString.Value));
            var @try = await entity.Select(async transaction =>
            {
                var inserted = await this.upsertBudget.Execute(NewUpsertParam(this.GetUser(), new Guid(id), request));
                return inserted.Select(_ => transaction);
            });

            return @try.Match(
                this.Error<BudgetModel>,
                transaction => this.NoContent());
        }

        /// <summary>
        /// Exclude.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteBudget([FromRoute] string id)
        {
            var deleted = await this.deleteBudget.Execute(NewDeleteParam(this.GetUser(), new Guid(id)));

            return deleted.Match(
                this.Error<BudgetModel>,
                _ => this.NoContent());
        }
    }
}
