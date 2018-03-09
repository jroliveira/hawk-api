namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities;

    internal sealed class TagProfile : Profile
    {
        public TagProfile()
        {
            this.CreateMap<(Tag Tag, int Count), Models.Tag.Get.Tag>()
                .ConstructUsing(item => new Models.Tag.Get.Tag(item.Tag.Name, item.Count));
        }
    }
}