using System.Threading.Tasks;

namespace RobinhoodCli.Commands
{
    public interface ICommand
    {
        Task<ExecutionResult> Execute(string authenticationToken);
    }
}