using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

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
            ExecutionContext context)
        {
            var result = await client.Login(Username, Password);
            if (!result.IsSuccessStatusCode)
            {
                context.ReplaceCommandQueueWithDisplayError($"Login failed: {result.Content}");
                return;
            }

            Console.WriteLine("Login successful");
            Console.WriteLine();
            if (SaveAuthenticationToken)
            {
                context.CommandQueue.Enqueue(new SaveAuthenticationTokenCommand(result.Data.Token));
            }
            context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(result.Data.Token));
        }

    }
}