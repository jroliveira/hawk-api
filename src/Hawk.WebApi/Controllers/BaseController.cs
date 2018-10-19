namespace Hawk.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : ControllerBase
    {
        protected CreatedResult Created(object id, object value)
        {
            var uri = $"{this.Request.GetDisplayUrl()}/{id}";

            return base.Created(uri, value);
        }
    }
}