namespace Hawk.Domain.Category
{
    using System.Linq;

    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.Extensions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Constants.ErrorMessages;
    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class Icon : ValueObject<Icon, string>
    {
        public Icon(in string name)
            : base(name.ToKebabCase())
        {
        }

        public static Try<Icon> NewIcon(in Option<string> valueOption) => valueOption
            .Fold(Failure<Icon>(IsRequired(nameof(Icon))))(value => new Icon(value));
    }
}
