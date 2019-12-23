namespace Hawk.WebApi.Infrastructure.Swagger
{
    internal sealed class SwaggerConfiguration
    {
        public bool? Enabled { get; set; }

        internal bool IsEnabled() => this.Enabled.GetValueOrDefault(false);
    }
}
