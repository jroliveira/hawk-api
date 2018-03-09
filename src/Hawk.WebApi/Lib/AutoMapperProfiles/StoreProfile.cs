namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities;

    internal sealed class StoreProfile : Profile
    {
        public StoreProfile()
        {
            this.CreateMap<Models.Store.Post.Store, Store>()
                .ConstructUsing(model => new Store(model.Name));

            this.CreateMap<(Store Store, int Count), Models.Store.Get.Store>()
                .ConstructUsing(entity => new Models.Store.Get.Store(entity.Store.Name, entity.Count));
        }
    }
}