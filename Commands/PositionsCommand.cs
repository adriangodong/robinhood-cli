using System;

namespace RobinhoodCli.Commands
{
    public class PositionsCommand : ICommand, ICommandParser
    {

        public void Execute()
        {
            Console.WriteLine("Open positions:");
        }

        public ICommand Parse(string[] commandTokens)
        {
            if (commandTokens[0] == "pos")
            {
                return new PositionsCommand();
            }

            return null;
        }

        public string LastError => null;

    }
}