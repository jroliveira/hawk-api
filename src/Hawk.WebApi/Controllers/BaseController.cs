namespace Hawk.WebApi.Controllers
{
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc;

    /// <inheritdoc />
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public CreatedResult Created(object id, object value)
        {
            var uri = $"{this.Request.GetDisplayUrl()}/{id}";

            return base.Created(uri, value);
        }
    }
}