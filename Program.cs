using System;

namespace RobinhoodCli
{
    public class Program
    {
        private static CommandParser commandParser = new CommandParser();

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
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    parsedCommand.Execute();
                }
            }
        }
    }
}
