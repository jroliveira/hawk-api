namespace Finance.WebApi.Lib.Filters
{
    using System.Collections.Generic;

    using Finance.WebApi.Models;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IDictionary<string, int> statusCode;

        public GlobalExceptionFilter()
        {
            this.statusCode = new Dictionary<string, int>
            {
                { "ValidationException", 409 }
            };
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var model = new Error
            {
                Message = exception.Message
            };

            this.statusCode.TryGetValue(exception.GetType().Name, out int code);
            context.Result = new ObjectResult(model)
            {
                StatusCode = code == 0 ? 500 : code
            };
        }
    }
}
