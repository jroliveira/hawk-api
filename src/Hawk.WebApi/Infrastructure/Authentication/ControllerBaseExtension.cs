namespace Hawk.WebApi.Infrastructure.Authentication
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    using Microsoft.AspNetCore.Mvc;

    using static System.String;

    using static Hawk.Domain.Shared.Email;

    internal static class ControllerBaseExtension
    {
        private const string Claim = "email";
        private const string Header = "X-Email";

        internal static Try<Email> GetUser(this ControllerBase @this)
        {
            if (@this.User.HasClaim(match => match.Type == Claim))
            {
                return NewEmail(@this
                    .User
                    .FindFirst(match => match.Type == Claim)
                    .Value);
            }

            if (@this.Request.Headers.TryGetValue(Header, out var email))
            {
                return NewEmail(email.ToString());
            }

            return NewEmail(Empty);
        }
    }
}
