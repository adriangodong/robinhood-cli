using System.Threading.Tasks;
using Deadlock.Robinhood;
using Deadlock.Robinhood.Model;
using RobinhoodCli.Models;
using RobinhoodCli.Services;

namespace RobinhoodCli.Commands
{
    internal class UpdateOpenOrdersCommand : ActiveAccountRequiredCommand
    {

        public override async Task ExecuteWithActiveAccount(
            IRobinhoodClient client,
            IOutputService output,
            ExecutionContext context,
            Account activeAccount)
        {
            var ordersResult = await client.Orders();
            if (!ordersResult.IsSuccessStatusCode)
            {
                output.Error(ordersResult.Content);
                return;
            }

            context.ClearOpenOrders();
            foreach (var order in ordersResult.Data.Results)
            {
                // TODO: better filter out orders (by account, include pending, etc)

                if (order.State == "confirmed")
                {
                    var instrumentResult = await client.Instrument(order.GetInstrumentKey());
                    if (!instrumentResult.IsSuccessStatusCode)
                    {
                        // TODO: replace with warning
                        output.Error(instrumentResult.Content);
                        return;
                    }

                    context.AddOpenOrder(order, instrumentResult.Data);
                }
            }

            output.OpenOrders(context.OpenOrders);
            output.ExitCommand();
        }

    }
}