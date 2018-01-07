namespace Hawk.WebApi.Lib.Extensions
{
    using System.Security.Claims;

    internal static class ClaimsPrincipalExtensions
    {
        public static string GetClientId(this ClaimsPrincipal claims)
        {
            const string ClientId = "client_id";

            if (!claims.HasClaim(match => match.Type == ClientId))
            {
                return string.Empty;
            }

            return claims
                .FindFirst(match => match.Type == "client_id")
                .Value;
        }
    }
}
