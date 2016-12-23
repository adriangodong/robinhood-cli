using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal interface ICommand
    {
        Task Execute(
            IRobinhoodClient client,
            ExecutionContext context);
    }
}