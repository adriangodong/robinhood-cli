using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal interface ICommand
    {
        Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context);
    }
}