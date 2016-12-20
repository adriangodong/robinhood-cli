using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public interface ICommand
    {
        Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context);
    }
}