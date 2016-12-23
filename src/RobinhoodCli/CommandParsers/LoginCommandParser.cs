using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal class LoginCommandParser : ICommandParser
    {

        public const string Error_MissingParameter = "Missing parameter(s) - login (username) (password)";
        public const string Error_MissingToken = "Missing parameter - login-token (token)";
        public const string Error_InvalidTokenFormat = "Expected 40 characters authentication token, received {0} characters";

        public ICommand Parse(string[] commandTokens)
        {
            LastError = null;

            if (commandTokens[0] == "login")
            {
                if (commandTokens.Length == 3)
                {
                    return new LoginCommand(commandTokens[1], commandTokens[2]);
                }

                LastError = Error_MissingParameter;
                return null;
            }

            if (commandTokens[0] == "login-token")
            {
                if (commandTokens.Length == 2)
                {
                    if (commandTokens[1].Length != 40)
                    {
                        LastError = string.Format(Error_InvalidTokenFormat, commandTokens[1].Length);
                        return null;
                    }

                    return new SetAuthenticationTokenCommand(commandTokens[1]);
                }

                LastError = Error_MissingToken;
                return null;
            }

            return null;
        }

        public string LastError { get; private set; }

    }
}