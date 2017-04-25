namespace Finance.Entities
{
    using Finance.Infrastructure.Security;

    public class Account : Entity<int>
    {
        private readonly IHashAlgorithm hashAlgorithm;

        public Account(string email, string password)
            : this(default(int), email, password)
        {
        }

        public Account(int id, string email, string password)
            : this(new Md5HashAlgorithm(), id, email, password)
        {
        }

        internal Account(IHashAlgorithm hashAlgorithm, int id, string email, string password)
        {
            this.hashAlgorithm = hashAlgorithm;
            this.Id = id;
            this.Email = email;
            this.Password = password;
        }

        public string Email { get; }

        public string Password { get; private set; }

        public virtual void HashPassword()
        {
            this.Password = this.hashAlgorithm.Hash(this.Password);
        }

        public virtual bool ValidatePassword(string password)
        {
            password = this.hashAlgorithm.Hash(password);

            return this.Password == password;
        }
    }
}
