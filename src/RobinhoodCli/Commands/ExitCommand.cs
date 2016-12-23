using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class ExitCommand : ICommand
    {
        public Task Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            System.Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}