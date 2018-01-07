namespace Hawk.WebApi.Models.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Account
    {
        /// <summary>
        /// 
        /// </summary>
        public Account(string email)
        {
            this.Email = email;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Email { get; }
    }
}