namespace Hawk.WebApi.Infrastructure.Authentication
{
    using System.Linq;
    using System.Threading.Tasks;

    using Hawk.Infrastructure.ErrorHandling.Exceptions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;

    using static Hawk.WebApi.Infrastructure.ErrorHandling.ErrorHandler;

    public class CustomAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService authService;
        private readonly string policyName;

        public CustomAuthorizeFilter(IAuthorizationService authService, string policyName)
        {
            this.authService = authService;
            this.policyName = policyName;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(meta => meta is IAllowAnonymous))
            {
                return;
            }

            var user = context.HttpContext.User;

            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = ErrorResult(new UnauthorizedException("Unauthorized."));
                return;
            }

            var authResult = await this.authService.AuthorizeAsync(user, default, this.policyName);

            if (!authResult.Succeeded)
            {
                context.Result = ErrorResult(new ForbiddenException("Forbidden."));
            }
        }
    }
}
