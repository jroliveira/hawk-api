namespace Hawk.WebApi.Models.Store.Post
{
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public sealed class Store
    {
        public string Name { get; set; }

        public static implicit operator Option<Domain.Entities.Store>(Store model) => Domain.Entities.Store.CreateWith(model.Name);

        public static implicit operator Domain.Entities.Store(Store model) => Domain.Entities.Store.CreateWith(model.Name).GetOrElse(default);
    }
}
