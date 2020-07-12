namespace Hawk.WebApi.Features.Tag
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Tag;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Tag.Tag;

    public class CreateTagModel
    {
        public CreateTagModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Tag>(in CreateTagModel model) => NewTag(model.Name);
    }
}
