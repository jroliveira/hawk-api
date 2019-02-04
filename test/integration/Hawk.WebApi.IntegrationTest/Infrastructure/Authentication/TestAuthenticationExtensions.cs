namespace Hawk.WebApi.IntegrationTest.Infrastructure.Authentication
{
    using System;

    using Microsoft.AspNetCore.Authentication;

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder @this, Action<TestAuthenticationOptions> configureOptions) => @this
            .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test Scheme", "Test Auth", configureOptions);
    }
}
