﻿namespace Hawk.WebApi.Infrastructure.Api
{
    using Hawk.Domain.Shared.Exceptions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;

    using static Hawk.WebApi.Infrastructure.ErrorHandling.ErrorHandler;

    internal sealed class VersioningErrorResponseProvider : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context) => ErrorResult(new InvalidRequestException(context.Message));
    }
}
