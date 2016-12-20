using System;
using System.Collections.Generic;
using Deadlock.Robinhood;
using RobinhoodCli.CommandParsers;
using RobinhoodCli.Commands;
using RobinhoodCli.ExecutionResults;
using RobinhoodCli.Models;

namespace RobinhoodCli
{
    public class Program
    {

        private static CommandParser commandParser = new CommandParser(new List<ICommandParser>() {
            new LoginCommandParser(),
            new OrderCommandParser()
        });

        private static ExecutionContext ExecutionContext;

        public static void Main(string[] args)
        {
            ExecutionContext = new ExecutionContext();
            Console.WriteLine("Robinhood CLI. Press Ctrl+C to exit.");

            while (true)
            {
                ICommand commandToExecute;

                // If there are no command in the queue, ask for a new one from user
                if (ExecutionContext.CommandQueue.Count == 0)
                {
                    Console.Write("> ");
                    var command = Console.ReadLine();
                    commandToExecute = commandParser.Parse(command);
                }
                else
                {
                    commandToExecute = ExecutionContext.CommandQueue.Dequeue();
                }

                if (commandToExecute == null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"ERROR: {commandParser.LastError}");
                    Console.ResetColor();
                }
                else
                {
                    using (IRobinhoodClient client = new RobinhoodClient(ExecutionContext.AuthenticationToken))
                    {
                        var task = commandToExecute.Execute(client, ExecutionContext);
                        task.Wait();
                        task.Result.UpdateExecutionContext(ExecutionContext);
                        task.Result.RenderResult();
                    }
                }
            }
        }
    }
}
