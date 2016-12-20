using System;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Models;

namespace RobinhoodCli.ExecutionResults
{
    public class AccountsExecutionResult : ExecutionResult
    {
        public Account Account { get; set; }

        public override void UpdateExecutionContext(ExecutionContext executionContext)
        {
            executionContext.ActiveAccount = Account;
        }

        public override void RenderResult()
        {
            Console.WriteLine($"Active account is {Account.AccountNumber}");
        }
    }
}