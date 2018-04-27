namespace Hawk.Infrastructure.Monad.Utils
{
    public static partial class Util
    {
        private static readonly Unit Default = default;

        public static Unit Unit() => Default;
    }
}