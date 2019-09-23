namespace Hawk.WebApi.Infrastructure.Hal.Resource
{
    using Hawk.WebApi.Infrastructure.Hal.Link;

    internal interface IResource
    {
        Links GetLinks();
    }
}
