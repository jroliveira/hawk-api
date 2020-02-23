namespace Hawk.Domain.Account.Queries
{
    using Hawk.Domain.Shared;
    using Hawk.Infrastructure.ErrorHandling.Exceptions;
    using Hawk.Infrastructure.Monad;

    using static Hawk.Infrastructure.Monad.Utils.Util;

    public class GetAccountByEmailParam : Param
    {
        private GetAccountByEmailParam(Email email)
            : base(email)
        {
        }

        public static Try<GetAccountByEmailParam> NewGetAccountByEmailParam(Option<Email> email) =>
                email
                    ? new GetAccountByEmailParam(email.Get())
                    : Failure<GetAccountByEmailParam>(new InvalidObjectException("Invalid get account by email param."));
    }
}
