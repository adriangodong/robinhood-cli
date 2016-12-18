using System;

namespace RobinhoodCli.Commands
{
    public class HelpCommand : ICommand, ICommandParser
    {

        public void Execute()
        {
            Console.WriteLine("Help");
        }

        public ICommand Parse(string[] commandTokens)
        {
            if (commandTokens[0] == "help")
            {
                return new HelpCommand();
            }
            return null;
        }

        public string LastError => null;

    }
}