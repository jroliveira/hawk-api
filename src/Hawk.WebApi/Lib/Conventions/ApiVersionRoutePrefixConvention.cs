namespace Hawk.WebApi.Lib.Conventions
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc.ApplicationModels;

    internal sealed class ApiVersionRoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel versionConstraintTemplate;

        internal ApiVersionRoutePrefixConvention() => this.versionConstraintTemplate = new AttributeRouteModel
        {
            Template = "v{api-version:apiVersion}",
        };

        public void Apply(ApplicationModel application)
        {
            foreach (var selector in application.Controllers
                .SelectMany(controller => controller.Selectors)
                .Where(selector => selector.AttributeRouteModel != null))
            {
                selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(this.versionConstraintTemplate, selector.AttributeRouteModel);
            }
        }
    }
}
