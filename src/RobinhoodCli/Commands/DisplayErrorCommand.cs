using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class DisplayErrorCommand : ICommand
    {

        private readonly string error;

        public DisplayErrorCommand(string error)
        {
            this.error = error;
        }

        public Task Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {error}");
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}