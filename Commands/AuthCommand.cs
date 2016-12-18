using System;
using System.Threading.Tasks;
using Deadlock.Robinhood;

namespace RobinhoodCli.Commands
{
    public class AuthCommand : ICommand, ICommandParser
    {

        public const string Error_MissingParameter = "Missing parameter(s) - login (username) (password)";

        public string Username { get; private set; }
        public string Password { get; private set; }
        public string AuthenticationToken { get; private set; }

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
                if (result.IsSuccessStatusCode)
                {
                    Console.WriteLine("Login successful");
                    return new AuthenticationExecutionResult()
                    {
                        AuthenticationToken = result.Data.Token
                    };
                }
                else
                {
                    return new ExecutionResult()
                    {
                        LastError = $"Login failed: {result.Content}"
                    };
                }
            }
        }

        public ICommand Parse(string[] commandTokens)
        {
            LastError = null;

            if (commandTokens[0] == "login")
            {
                if (commandTokens.Length == 3)
                {
                    return new AuthCommand()
                    {
                        Username = commandTokens[1],
                        Password = commandTokens[2]
                    };
                }

                LastError = Error_MissingParameter;
                return null;
            }

            if (commandTokens[0] == "login-token")
            {
                if (commandTokens.Length == 2)
                {
                    return new AuthCommand()
                    {
                        AuthenticationToken = commandTokens[1]
                    };
                }

                LastError = Error_MissingParameter;
                return null;
            }

            return null;
        }

        public string LastError { get; private set; }

    }
}