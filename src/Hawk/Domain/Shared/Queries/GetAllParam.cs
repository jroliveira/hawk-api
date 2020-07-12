namespace Hawk.Domain.Shared.Queries
{
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using Http.Query.Filter;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public sealed class GetAllParam : Param
    {
        private GetAllParam(in Email email, in Filter filter)
            : base(email) => this.Filter = filter;

        public Filter Filter { get; }

        public static Try<GetAllParam> NewGetByAllParam(in Option<Email> emailOption, in Filter filter) => emailOption
            ? new GetAllParam(
                emailOption.Get(),
                filter)
            : Failure<GetAllParam>(new InvalidObjectException("Invalid get all param."));
    }
}
