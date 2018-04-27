﻿namespace Hawk.WebApi.Models.Store.Get
{
    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    public sealed class Store
    {
        public Store(string name)
            : this(name, default)
        {
        }

        public Store(string name, uint total)
        {
            this.Name = name;
            this.Total = total;
        }

        public string Name { get; }

        public uint Total { get; }

        public static implicit operator Store(Domain.Entities.Store entity) => new Store(entity.Name);

        public static implicit operator Option<Domain.Entities.Store>(Store model) => Domain.Entities.Store.CreateWith(model.Name);

        public static implicit operator Domain.Entities.Store(Store model) => Domain.Entities.Store.CreateWith(model.Name).GetOrElse(default);
    }
}
