using System;

namespace RobinhoodCli.Commands
{
    public class ExecutionResult
    {

        public string LastError { get; set; }

        public virtual void UpdateExecutionContext(ExecutionContext executionContext)
        {
        }

        public virtual void RenderResult()
        {
            if (LastError != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"ERROR: {LastError}");
                Console.ResetColor();
            }
        }

        public static ExecutionResult NoResult = new ExecutionResult();

    }
}