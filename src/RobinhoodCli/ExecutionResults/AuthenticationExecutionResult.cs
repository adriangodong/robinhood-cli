using System;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.ExecutionResults
{
    public class AuthenticationExecutionResult : ExecutionResult
    {
        public string AuthenticationToken { get; set; }

        public override void UpdateExecutionContext(ExecutionContext executionContext)
        {
            executionContext.AuthenticationToken = AuthenticationToken;

            // Update active account
            executionContext.CommandQueue.Enqueue(new AccountCommand());

            // Show active account open positions
            executionContext.CommandQueue.Enqueue(new PositionsCommand());
        }

        public override void RenderResult()
        {
            if (LastError == null)
            {
                Console.WriteLine("Login successful");
                return;
            }

            base.RenderResult();
        }
    }
}