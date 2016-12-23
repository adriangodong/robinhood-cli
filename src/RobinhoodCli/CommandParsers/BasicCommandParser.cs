using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal class BasicCommandParser : ICommandParser
    {

        public ICommand Parse(string[] commandTokens)
        {
            switch (commandTokens[0])
            {
                case "position":
                    return new PositionsCommand();
                case "exit":
                    return new ExitCommand();
                default:
                    return null;
            }
        }

        public string LastError => null;

    }
}