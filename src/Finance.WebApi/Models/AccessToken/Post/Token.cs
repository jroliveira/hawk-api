namespace Finance.WebApi.Models.AccessToken.Post
{
    public class Token
    {
        public virtual string AccessToken { get; set; }

        public virtual double ExpiresIn { get; set; }
    }
}