using System;
using System.Threading.Tasks;
using Deadlock.Robinhood.Model;
using Deadlock.Robinhood;
using RobinhoodCli.ExecutionResults;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    public class ExecuteOrderCommand : ICommand
    {
        public NewOrder NewOrder { get; private set; }

        public ExecuteOrderCommand(NewOrder newOrder)
        {
            NewOrder = newOrder;
        }

        public async Task<ExecutionResult> Execute(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sending order: {NewOrder.Side} {NewOrder.Symbol} - {NewOrder.Quantity} shares - ${NewOrder.Price} limit");
            Console.ForegroundColor = ConsoleColor.Black;

            var newOrderResult = await client.Orders(NewOrder);

            if (!newOrderResult.IsSuccessStatusCode)
            {
                return new ExecutionResult(newOrderResult.Content);
            }

            return new NewOrderExecutionResult(newOrderResult.Data);
        }
    }
}