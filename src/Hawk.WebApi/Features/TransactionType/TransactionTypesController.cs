namespace Hawk.WebApi.Features.TransactionType
{
    using System.Linq;

    using Hawk.Domain.Transaction;
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Pagination;
    using Hawk.WebApi.Features.Shared;

    using Microsoft.AspNetCore.Mvc;

    using static System.Enum;

    using static Hawk.WebApi.Features.TransactionType.TransactionTypeModel;

    [ApiController]
    [ApiVersion("1")]
    [Route("")]
    public class TransactionTypesController : BaseController
    {
        /// <summary>
        /// Get.
        /// </summary>
        /// <returns></returns>
        [HttpGet("transaction-types")]
        [ProducesResponseType(typeof(Try<Page<Try<TransactionTypeModel>>>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public IActionResult GetTags() => this.Ok(GetValues(typeof(TransactionType))
            .Cast<TransactionType>()
            .Select(entity => NewTransactionTypeModel(entity)));
    }
}
