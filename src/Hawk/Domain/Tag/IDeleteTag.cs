﻿namespace Hawk.Domain.Tag
{
    using System.Threading.Tasks;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Monad;

    public interface IDeleteTag
    {
        Task<Try<Unit>> Execute(Email email, string name);
    }
}
