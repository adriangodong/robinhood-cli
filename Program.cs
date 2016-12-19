using System;
using System.Collections.Generic;
using RobinhoodCli.Commands;

namespace RobinhoodCli
{
    public class Program
    {

        private static CommandParser commandParser = new CommandParser(new List<ICommandParser>() {
            new HelpCommand(),
            new AuthCommandParser(),
            new PositionsCommand(),
            new OrderCommandParser()
        });

        private static ExecutionContext ExecutionContext;

        public static void Main(string[] args)
        {
            ExecutionContext = new ExecutionContext();
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
                    var task = parsedCommand.Execute(ExecutionContext);
                    task.Wait();
                    task.Result.UpdateExecutionContext(ExecutionContext);
                    task.Result.RenderResult();
                }
            }
        }
    }
}
