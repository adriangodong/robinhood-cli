using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class ExitCommand : ICommand
    {
        public Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            System.Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}