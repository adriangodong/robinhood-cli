using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class InitCommand : ICommand
    {

        internal string AuthenticationToken { get; private set; }

        public InitCommand(IConfiguration configuration)
        {
            AuthenticationToken = configuration["RobinhoodCli:AuthenticationToken"];
        }

        public Task Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            Console.WriteLine("Robinhood CLI.");

            if (!string.IsNullOrEmpty(AuthenticationToken))
            {
                context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(AuthenticationToken));
            }

            return Task.CompletedTask;
        }

    }
}