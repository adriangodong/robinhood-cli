using System;
using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;

namespace RobinhoodCli.Commands
{
    internal class CancelOpenOrderCommand : AuthenticationRequiredCommand
    {

        internal int OpenOrderIndexToCancel { get; private set; }

        public CancelOpenOrderCommand(int openOrderIndexToCancel)
        {
            OpenOrderIndexToCancel = openOrderIndexToCancel;
        }

        public override async Task ExecuteWithAuthentication(
            IRobinhoodClient client,
            ExecutionContext context)
        {
            var openOrder = context.OpenOrders.FirstOrDefault(oo => oo.Index == OpenOrderIndexToCancel);

            if (openOrder == null)
            {
                context.ReplaceCommandQueueWithDisplayError($"Can't find open order index {OpenOrderIndexToCancel}");
                return;
            }

            Console.WriteLine($"Cancelling order {openOrder.Order.Id}...");
            var cancelOrderResult = await client.CancelOrder(openOrder.Order.Id);
            if (!cancelOrderResult.IsSuccessStatusCode)
            {
                context.ReplaceCommandQueueWithDisplayError(cancelOrderResult.Content);
                return;
            }

            Console.WriteLine($"Order cancelled.");
            context.CommandQueue.Enqueue(new UpdateOpenOrdersCommand());
        }

    }
}