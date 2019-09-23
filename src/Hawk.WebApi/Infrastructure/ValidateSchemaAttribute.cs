namespace Hawk.WebApi.Infrastructure
{
    using System;
    using System.IO;
    using System.Linq;

    using Hawk.Domain.Shared.Exceptions;
    using Hawk.WebApi.Infrastructure.ErrorHandling.ErrorModels;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using NJsonSchema.Generation;

    using static System.IO.SeekOrigin;
    using static System.String;

    using static Hawk.Infrastructure.JsonSettings;

    using static Newtonsoft.Json.Linq.JObject;
    using static NJsonSchema.JsonSchema;

    public class ValidateSchemaAttribute : ActionFilterAttribute
    {
        private readonly Type schemaType;

        public ValidateSchemaAttribute(Type schema) => this.schemaType = schema;

        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            actionExecutingContext.HttpContext.Request.Body.Seek(0, Begin);

            using (var reader = new StreamReader(actionExecutingContext.HttpContext.Request.Body))
            {
                var json = reader.ReadToEnd();
                if (IsNullOrWhiteSpace(json))
                {
                    actionExecutingContext.Result = new StatusCodeResult(422);
                    return;
                }

                var schema = FromType(this.schemaType, new JsonSchemaGeneratorSettings { SerializerSettings = JsonSerializerSettings });

                schema.AllowAdditionalProperties = false;

                var body = Parse(json);
                var errors = schema.Validate(body);

                if (errors.Any())
                {
                    actionExecutingContext.Result = new ObjectResult(new ConflictErrorModel(new InvalidObjectException(
                        "Unprocessable entity",
                        errors
                            .Select(error => (error.Property, error.Kind.ToString()))
                            .ToList())))
                    {
                        StatusCode = 422,
                    };
                }
            }
        }
    }
}
