using System;
using System.Collections.Generic;
using RobinhoodCli.Client;
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

        private static Func<string, IClient> clientFactory = (token) => new DeadlockWrapperClient(token);

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
                    var client = clientFactory(ExecutionContext.AuthenticationToken);
                    var task = commandToExecute.Execute(client, ExecutionContext);
                    task.Wait();
                    task.Result.UpdateExecutionContext(ExecutionContext);
                    task.Result.RenderResult();
                }
            }
        }
    }
}
