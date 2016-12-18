using System;

namespace RobinhoodCli.Commands
{
    public class HelpCommand : ICommand
    {

        public void Execute()
        {
            Console.WriteLine("Help");
        }

    }
}