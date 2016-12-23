using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class LogoutCommand : ICommand
    {

        public Task Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            Console.WriteLine("You have been logged out.");
            Console.WriteLine();

            context.CommandQueue.Enqueue(new SaveAuthenticationTokenCommand(null));
            context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(null));
            return Task.CompletedTask;
        }

    }
}