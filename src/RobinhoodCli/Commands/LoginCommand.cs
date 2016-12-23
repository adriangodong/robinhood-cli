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

        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
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
            context.CommandQueue.Enqueue(new SetAuthenticationTokenCommand(result.Data.Token));
        }

    }
}