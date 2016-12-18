using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class AuthCommand : ICommand
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthenticationToken { get; set; }

        public async Task<ExecutionResult> Execute(string authenticationToken)
        {
            if (AuthenticationToken != null)
            {
                return new AuthenticationExecutionResult()
                {
                    AuthenticationToken = AuthenticationToken
                };
            }

            Console.WriteLine($"Logging in as {Username}");

            using (RobinhoodClient client = new RobinhoodClient())
            {
                var result = await client.Login(Username, Password);
                if (!result.IsSuccessStatusCode)
                {
                    return new ExecutionResult()
                    {
                        LastError = $"Login failed: {result.Content}"
                    };
                }

                Console.WriteLine("Login successful");
                return new AuthenticationExecutionResult()
                {
                    AuthenticationToken = result.Data.Token
                };
            }
        }

    }
}