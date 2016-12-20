using System.Threading.Tasks;
using RobinhoodCli.Client;

namespace RobinhoodCli.Commands
{
    public interface ICommand
    {
        Task<ExecutionResult> Execute(
            IClient client,
            ExecutionContext context);
    }
}