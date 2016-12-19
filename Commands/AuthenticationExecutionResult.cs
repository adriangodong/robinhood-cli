using System;

namespace RobinhoodCli.Commands
{
    public class AuthenticationExecutionResult : ExecutionResult
    {
        public string AuthenticationToken { get; set; }

        public override void UpdateExecutionContext(ExecutionContext executionContext)
        {
            executionContext.AuthenticationToken = AuthenticationToken;
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