namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Microsoft.AspNetCore.Mvc;

    internal static class ControllerBaseExtension
    {
        private const string Claim = "email";

        internal static string GetUser(this ControllerBase @this)
        {
            if (!@this.User.HasClaim(match => match.Type == Claim))
            {
                return string.Empty;
            }

            return @this
                .User
                .FindFirst(match => match.Type == Claim)
                .Value;
        }
    }
}
