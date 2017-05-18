namespace Finance.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Finance.Entities.Transaction.Details;

    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            this.CreateMap<Models.Store.Get.Store, Store>()
                .ConstructUsing(model => new Store(model.Name));

            this.CreateMap<Store, Models.Store.Get.Store>()
                .ForMember(destination => destination.Name, origin => origin.MapFrom(source => source.Name));
        }
    }
}