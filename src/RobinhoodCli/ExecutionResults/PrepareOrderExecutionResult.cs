using Deadlock.Robinhood.Model;
using RobinhoodCli.Commands;
using RobinhoodCli.Models;

namespace RobinhoodCli.ExecutionResults
{
    public class PrepareOrderExecutionResult : ExecutionResult
    {
        private readonly NewOrder newOrder;

        public PrepareOrderExecutionResult(NewOrder newOrder)
        {
            this.newOrder = newOrder;
        }

        public override void UpdateExecutionContext(ExecutionContext executionContext)
        {
            executionContext.CommandQueue.Enqueue(new ExecuteOrderCommand(newOrder));
        }
    }
}