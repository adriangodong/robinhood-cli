using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.ExecutionResults;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    public interface ICommand
    {
        Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context);
    }
}