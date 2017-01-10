using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class InitCommand : ICommand
    {

        public const string AuthenticationTokenConfigurationKey = "RobinhoodCli:AuthenticationToken";

        internal string AuthenticationToken { get; private set; }

        public InitCommand(IConfiguration configuration)
        {
            AuthenticationToken = configuration?[AuthenticationTokenConfigurationKey];
        }

        public Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            output.Info("Welcome to Robinhood CLI.");

            if (!string.IsNullOrEmpty(AuthenticationToken))
            {
                context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(AuthenticationToken));
            }

            output.ExitCommand();
            return Task.CompletedTask;
        }

    }
}