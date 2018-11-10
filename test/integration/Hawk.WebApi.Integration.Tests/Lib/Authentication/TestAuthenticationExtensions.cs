namespace Hawk.WebApi.Integration.Tests.Lib.Authentication
{
    using System;

    using Microsoft.AspNetCore.Authentication;

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions) => builder
            .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test Scheme", "Test Auth", configureOptions);
    }
}