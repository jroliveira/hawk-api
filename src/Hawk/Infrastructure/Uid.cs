namespace Hawk.Infrastructure
{
    using System;

    internal static class Uid
    {
        internal static Func<Guid> NewGuid { get; set; } = Guid.NewGuid;
    }
}
