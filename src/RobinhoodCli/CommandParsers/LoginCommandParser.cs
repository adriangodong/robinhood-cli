using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal class LoginCommandParser : ICommandParser
    {

        public const string Error_MissingParameter = "Missing parameter(s) - login (username) (password)";

        public ICommand Parse(string[] commandTokens)
        {
            LastError = null;

            if (commandTokens[0] == "login")
            {
                if (commandTokens.Length < 3)
                {
                    LastError = Error_MissingParameter;
                    return null;
                }

                var saveAuthenticationToken = false;
                if (commandTokens.Length == 4 &&
                    commandTokens[3] == "--save")
                {
                    saveAuthenticationToken = true;
                }

                return new LoginCommand(
                    commandTokens[1],
                    commandTokens[2],
                    saveAuthenticationToken);

            }

            if (commandTokens[0] == "logout")
            {
                return new LogoutCommand();
            }

            return null;
        }

        public string LastError { get; private set; }

    }
}