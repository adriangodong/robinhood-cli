using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

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
            ExecutionContext context)
        {
            context.AuthenticationToken = AuthenticationToken;

            // Update active account
            context.CommandQueue.Enqueue(new AccountCommand());

            // Show active account open positions
            context.CommandQueue.Enqueue(new UpdateOpenPositionsCommand());

            // Show active account open orders
            context.CommandQueue.Enqueue(new UpdateOpenOrdersCommand());

            return Task.CompletedTask;
        }

    }
}