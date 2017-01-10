using System.Threading.Tasks;
using Deadlock.Robinhood.Model;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

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
            IOutputService output,
            ExecutionContext context)
        {
            output.Success($"Sending order: {newOrder.Side} {newOrder.Symbol} - {newOrder.Quantity} shares - ${newOrder.Price} limit");

            var newOrderResult = await client.Orders(newOrder);

            if (!newOrderResult.IsSuccessStatusCode)
            {
                output.Error(newOrderResult.Content);
                return;
            }

            output.Info("Order placed!");
            output.ExitCommand();
            context.CommandQueue.Enqueue(new UpdateOpenOrdersCommand());
        }

    }
}