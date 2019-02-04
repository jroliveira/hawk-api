namespace Hawk.WebApi.Features.Shared.ErrorModels
{
    using Hawk.Domain.Shared.Exceptions;

    public class ConflictErrorModel
    {
        public ConflictErrorModel(InvalidObjectException exception)
        {
            if (exception == null)
            {
                return;
            }

            this.Message = exception.Message;
            this.Properties = exception.Properties;
        }

        public string Message { get; }

        public object Properties { get; }
    }
}
