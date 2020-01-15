namespace Hawk.Domain.Tag
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Tag : ValueObject<Tag, string>
    {
        private Tag(string name)
            : base(name)
        {
        }

        public static Try<Tag> NewTag(Option<string> name) =>
            name
            ? new Tag(name.Get())
            : Failure<Tag>(new InvalidObjectException("Invalid tag."));
    }
}
