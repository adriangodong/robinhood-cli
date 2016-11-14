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
                var order = commandParser.Parse(command);

                if (order == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"ERROR: {commandParser.LastError}");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Sending order: {order.Type} {order.Symbol} - {order.Size} shares - ${order.LimitPrice} limit");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
        }
    }
}
