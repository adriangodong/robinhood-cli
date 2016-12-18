using System;

namespace RobinhoodCli
{
    public class HelpCommand : ICommand
    {

        public void Execute()
        {
            Console.WriteLine("Help");
        }

    }
}