namespace Hawk.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    public class BaseController : Controller
    {
        /// <inheritdoc />
        public override CreatedResult Created(string id, object value)
        {
            var uri = $"{this.Request.GetDisplayUrl()}/{id}";

            return base.Created(uri, value);
        }
    }
}