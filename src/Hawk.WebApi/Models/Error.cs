namespace Hawk.WebApi.Models
{
    internal sealed class Error
    {
        public Error(string message)
        {
            this.Message = message;
        }

        public string Message { get; }
    }
}
