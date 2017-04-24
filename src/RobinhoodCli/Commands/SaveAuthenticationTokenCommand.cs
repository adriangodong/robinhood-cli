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
            var configJsonPath = $"{AppContext.BaseDirectory}{System.IO.Path.DirectorySeparatorChar}config.json";

            System.IO.File.WriteAllText(configJsonPath, configJson);
            output.Info($"Authentication saved in ${configJsonPath}.");
            output.ExitCommand();

            return Task.CompletedTask;
        }

    }
}