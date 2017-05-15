namespace Finance.WebApi.Lib.Mappings
{
    using AutoMapper;

    using Finance.Entities.Transaction.Details;

    public class TagProfile : Profile
    {
        public TagProfile()
        {
            this.CreateMap<Models.Tag.Get.Tag, Tag>()
                .ConstructUsing(model => new Tag(model.Name)
                {
                    Total = model.Total
                });

            this.CreateMap<Tag, Models.Tag.Get.Tag>()
                .ForMember(destination => destination.Name, origin => origin.MapFrom(source => source.Name));
        }
    }
}