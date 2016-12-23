using Deadlock.Robinhood.Model;
using RobinhoodCli.Models;

namespace RobinhoodCli.ExecutionResults
{
    public class NewOrderExecutionResult : ExecutionResult
    {
        private Order data;

        public NewOrderExecutionResult(Order data)
        {
            this.data = data;
        }

        public override void UpdateExecutionContext(ExecutionContext executionContext)
        {
        }
    }
}