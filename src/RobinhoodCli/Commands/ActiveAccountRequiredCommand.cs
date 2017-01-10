using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal abstract class ActiveAccountRequiredCommand : AuthenticationRequiredCommand
    {

        public const string Error_NoActiveAccount = "No active account selected or found.";

        public override Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            if (context.ActiveAccount == null)
            {
                context.CommandQueue.Clear();
                output.Error(Error_NoActiveAccount);
                return Task.CompletedTask;
            }

            return ExecuteWithActiveAccount(client, output, context, context.ActiveAccount);
        }

        public abstract Task ExecuteWithActiveAccount(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context,
            Account activeAccount);

    }
}