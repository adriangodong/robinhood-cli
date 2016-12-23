using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.ExecutionResults;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    public class ExitCommand : ICommand
    {
        public Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            System.Environment.Exit(0);
            return Task.FromResult(ExecutionResult.NoResult);
        }
    }
}