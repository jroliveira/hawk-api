namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Infrastructure;

    internal sealed class PagedProfile : Profile
    {
        public PagedProfile()
        {
            this.CreateMap(typeof(Paged<>), typeof(Paged<>));
        }
    }
}