using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal abstract class AuthenticationRequiredCommand : ICommand
    {

        public const string Error_Unauthenticated = "Session is not authenticated. Use login command to authenticate.";

        public Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            if (context.AuthenticationToken == null)
            {
                context.CommandQueue.Clear();
                output.Error(Error_Unauthenticated);
                return Task.CompletedTask;
            }

            return ExecuteWithAuthentication(client, output, context);
        }

        public abstract Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context);

    }
}