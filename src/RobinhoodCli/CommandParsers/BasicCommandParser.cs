using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    public class BasicCommandParser : ICommandParser
    {

        public ICommand Parse(string[] commandTokens)
        {
            switch (commandTokens[0])
            {
                case "exit":
                    return new ExitCommand();
                default:
                    return null;
            }
        }

        public string LastError => null;

    }
}