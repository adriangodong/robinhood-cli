using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal class QuoteCommandParser : ICommandParser
    {
        public const string Error_MissingSymbolParameter = "Missing symbol parameter";

        public ICommand Parse(string[] commandTokens)
        {
            if (commandTokens[0] == "quote" || commandTokens[0] == "q")
            {
                if (commandTokens.Length < 2)
                {
                    LastError = Error_MissingSymbolParameter;
                    return null;
                }

                return new QuoteCommand(commandTokens[1]);
            }

            return null;
        }

        public string LastError { get; private set; }

    }
}