using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class HelpCommand : ICommand, ICommandParser
    {

        public Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            Console.WriteLine("Help");
            return Task.FromResult(ExecutionResult.NoResult);
        }

        public ICommand Parse(string[] commandTokens)
        {
            if (commandTokens[0] == "help")
            {
                return new HelpCommand();
            }
            return null;
        }

        public string LastError => null;

    }
}