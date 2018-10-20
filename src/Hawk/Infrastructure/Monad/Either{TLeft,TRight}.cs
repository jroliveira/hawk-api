namespace Hawk.Infrastructure.Monad
{
    using System;

    public readonly struct Either<TLeft, TRight>
    {
        private readonly bool isLeft;
        private readonly TLeft left;
        private readonly TRight right;

        private Either(TLeft left)
        {
            this.isLeft = true;
            this.left = left;
            this.right = default;
        }

        private Either(TRight right)
        {
            this.isLeft = false;
            this.right = right;
            this.left = default;
        }

        public static implicit operator Either<TLeft, TRight>(TLeft left) => new Either<TLeft, TRight>(left);

        public static implicit operator Either<TLeft, TRight>(TRight right) => new Either<TLeft, TRight>(right);

        public TReturn Match<TReturn>(Func<TLeft, TReturn> left, Func<TRight, TReturn> right) => this.isLeft
            ? left(this.left)
            : right(this.right);
    }
}
