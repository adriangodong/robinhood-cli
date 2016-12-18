namespace RobinhoodCli.Commands
{
    public class ExecutionResult
    {
        public string LastError { get; set; }

        public static ExecutionResult NoResult = new ExecutionResult();
    }
}