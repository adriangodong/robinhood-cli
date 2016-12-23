using System;
using System.Threading.Tasks;
using Deadlock.Robinhood.Model;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class ExecuteOrderCommand : AuthenticationRequiredCommand
    {

        private readonly NewOrder newOrder;

        public ExecuteOrderCommand(NewOrder newOrder)
        {
            this.newOrder = newOrder;
        }

        public override async Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Sending order: {newOrder.Side} {newOrder.Symbol} - {newOrder.Quantity} shares - ${newOrder.Price} limit");
            Console.ResetColor();

            var newOrderResult = await client.Orders(newOrder);

            if (!newOrderResult.IsSuccessStatusCode)
            {
                context.ReplaceCommandQueueWithDisplayError(newOrderResult.Content);
                return;
            }

            Console.WriteLine("Order placed!");
            context.CommandQueue.Enqueue(new UpdateOpenOrdersCommand());
        }

    }
}