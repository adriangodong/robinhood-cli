using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    internal interface ICommandParser
    {
        ICommand Parse(string[] commandTokens);
        string LastError { get; }
    }
}