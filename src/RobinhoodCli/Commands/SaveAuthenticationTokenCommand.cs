using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class SaveAuthenticationTokenCommand : ICommand
    {

        internal string AuthenticationToken { get; private set; }

        public SaveAuthenticationTokenCommand(string authenticationToken)
        {
            AuthenticationToken = authenticationToken;
        }

        public Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            var configJson = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                robinhoodCli = new
                {
                    authenticationToken = AuthenticationToken
                }
            });

            System.IO.File.WriteAllText($"{AppContext.BaseDirectory}\\config.json", configJson);
            Console.WriteLine("Authentication saved in config.json.");
            Console.WriteLine();

            return Task.CompletedTask;
        }

    }
}