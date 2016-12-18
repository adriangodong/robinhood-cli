using System;
using System.Collections.Generic;
using RobinhoodCli.Commands;

namespace RobinhoodCli
{
    public class Program
    {
        private static CommandParser commandParser = new CommandParser(new List<ICommandParser>() {
            new HelpCommand(),
            new AuthCommand(),
            new PositionsCommand(),
            new OrderCommandParser()
        });

        public static void Main(string[] args)
        {
            Console.WriteLine("Robinhood CLI. Press Ctrl+C to exit.");
            while (true)
            {
                Console.Write("> ");
                var command = Console.ReadLine();
                var parsedCommand = commandParser.Parse(command);

                if (parsedCommand == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"ERROR: {commandParser.LastError}");
                    Console.ResetColor();
                }
                else
                {
                    parsedCommand.Execute();
                }
            }
        }
    }
}
