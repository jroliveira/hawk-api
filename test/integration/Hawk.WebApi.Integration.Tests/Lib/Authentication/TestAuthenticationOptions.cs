namespace Hawk.WebApi.Integration.Tests.Lib.Authentication
{
    using System;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication;

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(
            new[]
            {
                new Claim(
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                    Guid.NewGuid().ToString()),
            },
            "test");
    }
}
