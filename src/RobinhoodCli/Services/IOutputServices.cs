namespace RobinhoodCli.Services
{
    internal interface IOutputService
    {
        void Info(string message);
        void Success(string message);
        void Error(string message);
        void ExitCommand();
    }
}