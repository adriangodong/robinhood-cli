using System;
using RobinhoodCli.Models;

namespace RobinhoodCli.ExecutionResults
{
    public class ExecutionResult
    {

        public string LastError { get; set; }

        public ExecutionResult()
        {
        }

        public ExecutionResult(string lastError)
        {
            LastError = lastError;
        }

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