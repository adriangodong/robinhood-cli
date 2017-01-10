using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class SetAuthenticationTokenCommand : ICommand
    {

        internal string AuthenticationToken { get; private set; }

        public SetAuthenticationTokenCommand(string authenticationToken)
        {
            AuthenticationToken = authenticationToken;
        }

        public Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            context.AuthenticationToken = AuthenticationToken;

            if (!string.IsNullOrEmpty(AuthenticationToken))
            {
                // Update active account
                context.CommandQueue.Enqueue(new AccountCommand());

                // Show active account open positions
                context.CommandQueue.Enqueue(new UpdateOpenPositionsCommand());

                // Show active account open orders
                context.CommandQueue.Enqueue(new UpdateOpenOrdersCommand());
            }
            else
            {
                // Unset active account
                context.ActiveAccount = null;
            }

            return Task.CompletedTask;
        }

    }
}