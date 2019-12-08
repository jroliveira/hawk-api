namespace Hawk.WebApi.Infrastructure.Swagger
{
    internal sealed class SwaggerConfiguration
    {
        public bool? Enabled { get; set; }

        internal bool IsEnabled()
        {
            if (!this.Enabled.GetValueOrDefault(false))
            {
                return false;
            }

            return false;
        }
    }
}
