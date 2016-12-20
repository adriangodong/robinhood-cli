using RobinhoodCli.Commands;

namespace RobinhoodCli.CommandParsers
{
    public interface ICommandParser
    {
        ICommand Parse(string[] commandTokens);
        string LastError { get; }
    }
}