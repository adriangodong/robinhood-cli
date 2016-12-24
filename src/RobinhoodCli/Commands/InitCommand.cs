using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

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
            ExecutionContext context)
        {
            Console.WriteLine("Welcome to Robinhood CLI.");
            Console.WriteLine();

            if (!string.IsNullOrEmpty(AuthenticationToken))
            {
                context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(AuthenticationToken));
            }

            return Task.CompletedTask;
        }

    }
}