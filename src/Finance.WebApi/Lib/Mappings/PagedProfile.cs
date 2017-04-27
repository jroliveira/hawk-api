namespace Finance.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Finance.Infrastructure;

    public class PagedProfile : Profile
    {
        public PagedProfile()
        {
            this.CreateMap(typeof(Paged<>), typeof(Paged<>));
        }
    }
}