using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal abstract class AuthenticationRequiredCommand : ICommand
    {

        public const string Error_Unauthenticated = "Session is not authenticated. Use login command to authenticate.";

        public Task Execute(IRobinhoodClient client, ExecutionContext context)
        {
            if (context.AuthenticationToken == null)
            {
                context.ReplaceCommandQueueWithDisplayError(Error_Unauthenticated);
                return Task.CompletedTask;
            }

            return ExecuteWithAuthentication(client, context);
        }

        public abstract Task ExecuteWithAuthentication(IRobinhoodClient client, ExecutionContext context);

    }
}