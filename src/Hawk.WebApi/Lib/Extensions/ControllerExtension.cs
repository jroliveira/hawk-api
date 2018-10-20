namespace Hawk.WebApi.Lib.Extensions
{
    using Microsoft.AspNetCore.Mvc;

    internal static class ControllerExtension
    {
        private const string ClientId = "client_id";

        internal static string GetUser(this ControllerBase @this)
        {
            if (!@this.User.HasClaim(match => match.Type == ClientId))
            {
                return string.Empty;
            }

            return @this
                .User
                .FindFirst(match => match.Type == "client_id")
                .Value;
        }
    }
}
