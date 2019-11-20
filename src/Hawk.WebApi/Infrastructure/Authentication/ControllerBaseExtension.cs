namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Microsoft.AspNetCore.Mvc;

    using static System.String;

    internal static class ControllerBaseExtension
    {
        private const string Claim = "email";
        private const string Header = "X-Email";

        internal static string GetUser(this ControllerBase @this)
        {
            if (@this.User.HasClaim(match => match.Type == Claim))
            {
                return @this
                    .User
                    .FindFirst(match => match.Type == Claim)
                    .Value;
            }

            if (@this.Request.Headers.TryGetValue(Header, out var email))
            {
                return email;
            }

            return Empty;
        }
    }
}
