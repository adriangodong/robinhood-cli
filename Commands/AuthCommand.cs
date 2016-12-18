using System;

namespace RobinhoodCli.Commands
{
    public class AuthCommand : ICommand, ICommandParser
    {

        public const string Error_MissingParameter = "Missing parameter(s) - login (username) (password)";

        public string Username { get; private set; }
        public string Password { get; private set; }

        public void Execute()
        {
            Console.WriteLine($"Logging in as {Username}");
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

            return null;
        }

        public string LastError { get; private set; }

    }
}