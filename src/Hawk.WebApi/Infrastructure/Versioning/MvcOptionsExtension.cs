namespace Hawk.WebApi.Infrastructure.Versioning
{
    using Microsoft.AspNetCore.Mvc;

    internal static class MvcOptionsExtension
    {
        internal static MvcOptions AddApiVersionRoutePrefixConvention(this MvcOptions @this)
        {
            @this
                .Conventions
                .Add(new ApiVersionRoutePrefixConvention());

            return @this;
        }
    }
}
