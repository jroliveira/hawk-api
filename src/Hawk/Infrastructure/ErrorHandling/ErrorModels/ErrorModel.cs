namespace Hawk.Infrastructure.ErrorHandling.ErrorModels
{
    public abstract class ErrorModel
    {
        protected ErrorModel(string message) => this.Message = message;

        public string Message { get; }
    }
}
