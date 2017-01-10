using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class LogoutCommand : ICommand
    {

        public Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            output.Info("You have been logged out.");
            output.ExitCommand();

            context.CommandQueue.Enqueue(new SaveAuthenticationTokenCommand(null));
            context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(null));
            return Task.CompletedTask;
        }

    }
}