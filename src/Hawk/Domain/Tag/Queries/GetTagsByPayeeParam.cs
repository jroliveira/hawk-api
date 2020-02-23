namespace Hawk.Domain.Tag.Queries
{
    using Hawk.Domain.Payee;
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class GetTagsByPayeeParam : Param
    {
        private GetTagsByPayeeParam(
            Email email,
            Payee payee,
            Filter filter)
            : base(email)
        {
            this.Payee = payee;
            this.Filter = filter;
        }

        public Payee Payee { get; }

        public Filter Filter { get; }

        public static Try<GetTagsByPayeeParam> NewGetTagsByPayeeParam(
            Option<Email> email,
            Option<Payee> payee,
            Filter filter) =>
                email
                && payee
                    ? new GetTagsByPayeeParam(
                        email.Get(),
                        payee.Get(),
                        filter)
                    : Failure<GetTagsByPayeeParam>(new InvalidObjectException("Invalid get tags by payee param."));
    }
}
