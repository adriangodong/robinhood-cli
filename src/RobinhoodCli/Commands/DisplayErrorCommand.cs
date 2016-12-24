using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class DisplayErrorCommand : ICommand
    {

        internal string Error { get; private set; }

        public DisplayErrorCommand(string error)
        {
            Error = error;
        }

        public Task Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {Error}");
            Console.WriteLine();
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}