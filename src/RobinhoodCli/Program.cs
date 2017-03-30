using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Deadlock.Robinhood;
using RobinhoodCli.CommandParsers;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli
{
    class Program
    {

        private static CommandParser commandParser = new CommandParser(new List<ICommandParser>() {
            new BasicCommandParser(),
            new LoginCommandParser(),
            new OrderCommandParser(),
            new QuoteCommandParser()
        });

        private static IOutputService Output;
        private static ExecutionContext ExecutionContext;

        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json", true)
                .Build();

            Output = new ConsoleOutputService();
            ExecutionContext = new ExecutionContext();
            ExecutionContext.CommandQueue.Enqueue(new InitCommand(configuration));

            Console.Clear();
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
                    Console.WriteLine();
                    Console.ResetColor();
                }
                else
                {
                    using (IRobinhoodClient client = new RobinhoodClient(ExecutionContext.AuthenticationToken))
                    {
                        commandToExecute.Execute(client, Output, ExecutionContext).Wait();
                    }
                }
            }
        }

    }
}
