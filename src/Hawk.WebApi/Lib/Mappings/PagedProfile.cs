namespace Hawk.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Hawk.Infrastructure;

    public class PagedProfile : Profile
    {
        public PagedProfile()
        {
            this.CreateMap(typeof(Paged<>), typeof(Paged<>));
        }
    }
}