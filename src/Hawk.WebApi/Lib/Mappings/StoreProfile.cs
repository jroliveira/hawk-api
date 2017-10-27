namespace Hawk.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Hawk.Entities.Transaction.Details;

    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            this.CreateMap<Models.Store.Get.Store, Store>()
                .ConstructUsing(model => new Store(model.Name));

            this.CreateMap<Store, Models.Store.Get.Store>();

            this.CreateMap<Store, GraphQl.Sources.Store>();
        }
    }
}