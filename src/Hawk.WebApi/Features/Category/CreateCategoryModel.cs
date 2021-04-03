namespace Hawk.WebApi.Features.Category
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Category;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Category.Category;
    using static Hawk.Domain.Category.Icon;

    public class CreateCategoryModel
    {
        public CreateCategoryModel(
            string name,
            string icon)
        {
            this.Name = name;
            this.Icon = icon;
        }

        [Required]
        public string Name { get; }

        [Required]
        public string Icon { get; }

        public static implicit operator Option<Category>(in CreateCategoryModel model) => NewCategory(
            model.Name,
            NewIcon(model.Icon));
    }
}
