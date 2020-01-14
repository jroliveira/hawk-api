namespace Hawk.WebApi.Features.Tag
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Tag.Tag;

    public class NewTagModel
    {
        public NewTagModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Tag>(NewTagModel model) => MapNewTag(model);

        public static implicit operator NewTagModel(Tag entity) => new NewTagModel(entity.Name);

        public static Option<Tag> MapNewTag(NewTagModel model) => NewTag(model.Name);
    }
}
