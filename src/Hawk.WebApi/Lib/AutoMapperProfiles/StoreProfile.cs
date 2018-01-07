namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities;

    internal sealed class StoreProfile : Profile
    {
        public StoreProfile()
        {
            this.CreateMap<Models.Store, Store>()
                .ConstructUsing(model => new Store(model.Name));

            this.CreateMap<Store, Models.Store>()
                .ConstructUsing(entity => new Models.Store(entity.Name, entity.Total));
        }
    }
}