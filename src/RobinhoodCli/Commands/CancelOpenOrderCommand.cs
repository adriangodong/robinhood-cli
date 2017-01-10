using System.Linq;
using System.Threading.Tasks;
using Deadlock.Robinhood;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

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
            IOutputService output,
            ExecutionContext context)
        {
            var openOrder = context.OpenOrders.FirstOrDefault(oo => oo.Index == OpenOrderIndexToCancel);

            if (openOrder == null)
            {
                output.Error($"Can't find open order index {OpenOrderIndexToCancel}");
                return;
            }

            output.Info($"Cancelling order {openOrder.Order.Id}...");
            var cancelOrderResult = await client.CancelOrder(openOrder.Order.Id);
            if (!cancelOrderResult.IsSuccessStatusCode)
            {
                output.Error(cancelOrderResult.Content);
                return;
            }

            output.Info($"Order cancelled.");
            output.ExitCommand();
            context.CommandQueue.Enqueue(new UpdateOpenOrdersCommand());
        }

    }
}