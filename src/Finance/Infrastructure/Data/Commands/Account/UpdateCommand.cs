namespace Finance.Infrastructure.Data.Commands.Account
{
    using System.Threading.Tasks;

    using Finance.Entities;

    public class UpdateCommand
    {
        private readonly PartialUpdater partialUpdater;

        public UpdateCommand(PartialUpdater partialUpdater)
        {
            this.partialUpdater = partialUpdater;
        }

        protected UpdateCommand()
        {
        }

        public virtual async Task ExecuteAsync(Account entity, dynamic changedModel)
        {
            await Task.Run(() => this.partialUpdater.Apply(changedModel, entity));
        }
    }
}