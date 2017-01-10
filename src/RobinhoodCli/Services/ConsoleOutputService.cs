using System;

namespace RobinhoodCli.Services
{
    internal class ConsoleOutputService : IOutputService
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {message}");
            Console.WriteLine();
            Console.ResetColor();
        }

        public void ExitCommand()
        {
            Console.WriteLine();
        }
    }
}