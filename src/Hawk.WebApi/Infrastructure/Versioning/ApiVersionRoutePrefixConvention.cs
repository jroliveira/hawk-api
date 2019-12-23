namespace Hawk.WebApi.Infrastructure.Versioning
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.ApplicationModels;

    using static Microsoft.AspNetCore.Mvc.ApplicationModels.AttributeRouteModel;

    internal sealed class ApiVersionRoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel versionConstraintTemplate;

        internal ApiVersionRoutePrefixConvention() => this.versionConstraintTemplate = new AttributeRouteModel
        {
            Template = "v{version:apiVersion}",
        };

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers
                .SelectMany(controller => controller.Selectors)
                .Where(selector => selector.AttributeRouteModel != default))
            {
                selector.AttributeRouteModel = CombineAttributeRouteModel(this.versionConstraintTemplate, selector.AttributeRouteModel);
            }
        }
    }
}
