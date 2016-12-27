using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal abstract class ActiveAccountRequiredCommand : AuthenticationRequiredCommand
    {

        public const string Error_NoActiveAccount = "No active account selected or found.";

        public override Task ExecuteWithAuthentication(IRobinhoodClient client, ExecutionContext context)
        {
            if (context.ActiveAccount == null)
            {
                context.ReplaceCommandQueueWithDisplayError(Error_NoActiveAccount);
                return Task.CompletedTask;
            }

            return ExecuteWithActiveAccount(client, context, context.ActiveAccount);
        }

        public abstract Task ExecuteWithActiveAccount(IRobinhoodClient client, ExecutionContext context, Account activeAccount);

    }
}