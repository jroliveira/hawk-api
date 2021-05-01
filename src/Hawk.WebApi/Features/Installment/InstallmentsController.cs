namespace Hawk.WebApi.Features.Installment
{
    using System;
    using System.Threading.Tasks;

    using Hawk.Domain.Installment.Commands;
    using Hawk.Domain.Installment.Queries;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;
    using Hawk.WebApi.Infrastructure.Authentication;

    using Microsoft.AspNetCore.Mvc;

    using static Hawk.Domain.Shared.Commands.DeleteParam<System.Guid>;
    using static Hawk.Domain.Shared.Queries.GetAllParam;
    using static Hawk.Domain.Shared.Queries.GetByIdParam<System.Guid>;
    using static Hawk.Infrastructure.Monad.Utils.Util;
    using static Hawk.WebApi.Features.Installment.InstallmentModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("installments")]
    public class InstallmentsController : BaseController
    {
        private readonly IGetInstallments getInstallments;
        private readonly IGetInstallmentById getInstallmentById;
        private readonly IDeleteInstallment deleteInstallment;

        public InstallmentsController(
            IGetInstallments getInstallments,
            IGetInstallmentById getInstallmentById,
            IDeleteInstallment deleteInstallment)
        {
            this.getInstallments = getInstallments;
            this.getInstallmentById = getInstallmentById;
            this.deleteInstallment = deleteInstallment;
        }

        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Try<Page<Try<InstallmentModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInstallments()
        {
            var entities = await this.getInstallments.GetResult(NewGetByAllParam(this.GetUser(), this.Request.QueryString.Value));

            return entities.Match(
                this.Error<Page<Try<InstallmentModel>>>,
                page => this.Ok(page.ToPage(entity => NewInstallmentModel(entity))));
        }

        /// <summary>
        /// Get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Try<InstallmentModel>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInstallmentById([FromRoute] string id)
        {
            var entity = await this.getInstallmentById.GetResult(NewGetByIdParam(this.GetUser(), new Guid(id)));

            return entity.Match(
                this.Error<InstallmentModel>,
                installment => this.Ok(Success(NewInstallmentModel(installment))));
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
        public async Task<IActionResult> DeleteInstallment([FromRoute] string id)
        {
            var deleted = await this.deleteInstallment.Execute(NewDeleteParam(this.GetUser(), new Guid(id)));

            return deleted.Match(
                this.Error<InstallmentModel>,
                _ => this.NoContent());
        }
    }
}
