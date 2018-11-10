namespace Hawk.Domain.Account
{
    using System;

    using Hawk.Infrastructure.Monad;
    using Hawk.Infrastructure.Monad.Extensions;

    using static System.String;

    public sealed class Account : Entity<Guid>
    {
        private Account(Guid id, string email)
        {
            this.Id = id;
            this.Email = email;
        }

        public string Email { get; }

        public static Try<Account> CreateWith(Option<string> nameOption) => CreateWith(Guid.NewGuid(), nameOption);

        public static Try<Account> CreateWith(Option<Guid> accountIdOption, Option<string> nameOption)
        {
            var id = accountIdOption.GetOrElse(Guid.Empty);
            if (id == Guid.Empty)
            {
                return new ArgumentNullException(nameof(id), "Account's id cannot be null.");
            }

            var name = nameOption.GetOrElse(Empty);
            if (IsNullOrEmpty(name))
            {
                return new ArgumentNullException(nameof(name), "Account's e-mail cannot be null or empty.");
            }

            return new Account(id, name);
        }
    }
}
