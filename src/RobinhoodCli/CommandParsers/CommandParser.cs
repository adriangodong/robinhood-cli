using System;
using System.Collections.Generic;
using System.Linq;
using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal class CommandParser
    {

        public const string Error_EmptyCommand = "Empty command";
        public const string Error_UnknownFirstToken = "Unknown first token '{0}'";

        private List<ICommandParser> commandParsers = new List<ICommandParser>();

        public CommandParser(IEnumerable<ICommandParser> commandParsers)
        {
            this.commandParsers = commandParsers.ToList();
        }

        public ICommand Parse(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                LastError = Error_EmptyCommand;
                return null;
            }

            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var commandParser in commandParsers)
            {
                var parsedCommand = commandParser.Parse(tokens);
                if (parsedCommand != null)
                {
                    return parsedCommand;
                }
                else if (commandParser.LastError != null)
                {
                    LastError = commandParser.LastError;
                    return null;
                }
            }

            LastError = string.Format(Error_UnknownFirstToken, tokens[0]);
            return null;
        }

        public string LastError { get; private set; }

    }
}