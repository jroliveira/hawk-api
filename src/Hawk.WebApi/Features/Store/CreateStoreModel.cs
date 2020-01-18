namespace Hawk.WebApi.Features.Store
{
    using System.ComponentModel.DataAnnotations;

    using Hawk.Domain.Store;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Domain.Store.Store;

    public class CreateStoreModel
    {
        public CreateStoreModel(string name) => this.Name = name;

        [Required]
        public string Name { get; }

        public static implicit operator Option<Store>(CreateStoreModel model) => NewStore(model.Name);
    }
}
