using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal class BasicCommandParser : ICommandParser
    {

        public ICommand Parse(string[] commandTokens)
        {
            switch (commandTokens[0])
            {
                case "positions":
                    return new UpdateOpenPositionsCommand();
                case "orders":
                    return new UpdateOpenOrdersCommand();
                case "exit":
                    return new ExitCommand();
                default:
                    return null;
            }
        }

        public string LastError => null;

    }
}