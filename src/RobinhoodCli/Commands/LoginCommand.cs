using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class LoginCommand : ICommand
    {

        internal string Username { get; private set; }
        internal string Password { get; private set; }
        internal bool SaveAuthenticationToken { get; private set; }

        public LoginCommand(string username, string password, bool saveAuthenticationToken)
        {
            Username = username;
            Password = password;
            SaveAuthenticationToken = saveAuthenticationToken;
        }

        public async Task Execute(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context)
        {
            var result = await client.Login(Username, Password);
            if (!result.IsSuccessStatusCode)
            {
                output.Error($"Login failed: {result.Content}");
                return;
            }

            output.Info("Login successful");
            output.ExitCommand();
            if (SaveAuthenticationToken)
            {
                context.CommandQueue.Enqueue(new SaveAuthenticationTokenCommand(result.Data.Token));
            }
            context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(result.Data.Token));
        }

    }
}