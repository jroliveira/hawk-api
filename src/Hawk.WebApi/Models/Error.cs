namespace Hawk.WebApi.Models
{
    public sealed class Error
    {
        public Error(string message)
        {
            this.Message = message;
        }

        public string Message { get; }
    }
}
