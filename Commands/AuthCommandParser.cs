namespace RobinhoodCli.Commands
{
    public class AuthCommandParser : ICommandParser
    {

        public const string Error_MissingParameter = "Missing parameter(s) - login (username) (password)";
        public const string Error_MissingToken = "Missing parameter - login-token (token)";

        public ICommand Parse(string[] commandTokens)
        {
            LastError = null;

            if (commandTokens[0] == "login")
            {
                if (commandTokens.Length == 3)
                {
                    return new LoginCommand()
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
                    if (commandTokens[1].Length != 40)
                    {
                        LastError = $"Expected 40 characters authentication token, received {commandTokens[1].Length} characters";
                        return null;
                    }

                    return new AuthCommand()
                    {
                        AuthenticationToken = commandTokens[1]
                    };
                }

                LastError = Error_MissingToken;
                return null;
            }

            return null;
        }

        public string LastError { get; private set; }

    }
}