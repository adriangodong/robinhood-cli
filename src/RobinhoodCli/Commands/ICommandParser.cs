namespace RobinhoodCli.Commands
{
    public interface ICommandParser
    {
        ICommand Parse(string[] commandTokens);
        string LastError { get; }
    }
}