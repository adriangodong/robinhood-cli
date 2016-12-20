using System;

namespace RobinhoodCli.Commands
{
    public class AccountsExecutionResult : ExecutionResult
    {
        public string Account { get; set; }

        public override void UpdateExecutionContext(ExecutionContext executionContext)
        {
            executionContext.ActiveAccount = Account;
        }

        public override void RenderResult()
        {
            Console.WriteLine($"Active account is {Account}");
        }
    }
}