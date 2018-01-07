namespace Hawk.WebApi.Lib.AutoMapperProfiles
{
    using AutoMapper;

    using Hawk.Domain.Entities;

    internal sealed class TagProfile : Profile
    {
        public TagProfile()
        {
            this.CreateMap<Models.Tag, Tag>()
                .ConstructUsing(model => new Tag(model.Name));

            this.CreateMap<Tag, Models.Tag>()
                .ConstructUsing(entity => new Models.Tag(entity.Name, entity.Total));
        }
    }
}